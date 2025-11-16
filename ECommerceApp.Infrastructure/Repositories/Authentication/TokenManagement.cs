using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ECommerceApp.Domain.Entities.Identity;
using ECommerceApp.Domain.Interfaces.Authentication;
using ECommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ECommerceApp.Infrastructure.Repositories.Authentication;

public class TokenManagement(AppDbContext dbContext, IConfiguration config) : ITokenManagement
{
    public string GetRefreshToken()
    {
        const int byteSize = 64;
        Span<byte> buffer = stackalloc byte[byteSize];

        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(buffer);
        
        string token = Convert.ToBase64String(buffer);
        return WebUtility.UrlEncode(token);
    }

    public List<Claim> GetUserClaimsFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        if (jwtToken is not null)
            return jwtToken.Claims.ToList();
        else
            return [];
    }

    public async Task<bool> ValidateRefreshToken(string refreshToken)
    {
        var user = await dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken);
        return user !=  null;
    }

    public async Task<string> GetUserIdByRefreshToken(string refreshToken)
        => (await dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken))!.UserId.ToString();

    public async Task<int> AddRefreshToken(string userId, string refreshToken)
    {
        await dbContext.RefreshTokens.AddAsync(new RefreshToken()
        {
            UserId = userId,
            Token = refreshToken,
        });
        return await dbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateRefreshToken(string userId, string refreshToken)
    {
        var user = await dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken);
        if (user is null) return -1;

        user.Token = refreshToken;
        return await dbContext.SaveChangesAsync();
    }

    public string GenerateToken(List<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expirationDate = DateTime.UtcNow.AddHours(2);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: expirationDate,
            signingCredentials: cred
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
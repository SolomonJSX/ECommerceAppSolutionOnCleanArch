using ECommerceApp.Application.Services.Interfaces.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace ECommerceApp.Infrastructure.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DbUpdateException ex)
        {
            var logger = context.RequestServices.GetRequiredService<IAppLogger<ExceptionHandlingMiddleware>>();
            context.Response.ContentType = "application/json";
            if (ex.InnerException is PostgresException pgEx)
            {
                logger.LogError(pgEx, "Sql Exception");
                switch (pgEx.SqlState)
                {
                    case "23505": // unique_violation
                        context.Response.StatusCode = StatusCodes.Status409Conflict;
                        await context.Response.WriteAsync("Unique constraint violation.");
                        break;

                    case "23502": // not_null_violation
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Cannot insert null value.");
                        break;

                    case "23503": // foreign_key_violation
                        context.Response.StatusCode = StatusCodes.Status409Conflict;
                        await context.Response.WriteAsync("Foreign key constraint violation.");
                        break;

                    default:
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsync("A database error occurred.");
                        break;
                }
            }
            else
            {
                logger.LogError(ex, "Related EFCore Exception");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("An error occurred while saving the entity changes.");
            }
        }
        catch (Exception ex)
        {
            var logger = context.RequestServices.GetRequiredService<IAppLogger<ExceptionHandlingMiddleware>>();
            logger.LogError(ex, "Unknown exception.");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("An error occured: " + ex.Message);
        }
    }
}
using ECommerceApp.Application.Services.Interfaces.Logging;
using Microsoft.Extensions.Logging;

namespace ECommerceApp.Infrastructure.Services;

public class SerilogLoggerAdapter<T>(ILogger<T> logger) : IAppLogger<T>
{
    public void LogInformation(string message) => logger.LogInformation(message);

    public void LogWarning(string message) => logger.LogWarning(message);
    
    public void LogError(Exception exception, string message) => logger.LogError(exception, message);
}
using ECommerceApp.Application.DependencyInjection;
using ECommerceApp.Infrastructure.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("log/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

//builder.Host.UseSerilog();
Log.Logger.Information("Application is building.......");

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("https://localhost:")
            .AllowCredentials();
    });
});

try
{
    var app = builder.Build();
    app.UseCors();
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseInfrastructureServices();

    app.UseHttpsRedirection();


    app.UseAuthorization();
    app.MapControllers();
    Log.Logger.Information("Application is running.......");
    app.Run();
}
catch (Exception ex)
{
    Log.Logger.Error(ex, "Application failed to start.......");
}
finally
{
    Log.CloseAndFlush();
}


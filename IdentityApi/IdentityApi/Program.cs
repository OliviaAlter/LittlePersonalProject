using Identity.BuilderExtension;
using Identity.Health;
using Identity.Middleware;
using Identity.ServiceExtension;
using IdentityInfrastructure.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Configure Serilog
builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/myapp.txt",
            rollingInterval: RollingInterval.Day, // Creates a new file every day
            retainedFileCountLimit: 30, // Retains logs for the last 30 days
            outputTemplate:
            "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {CorrelationId} {Message:lj}{NewLine}{Exception}", // Use structured logging
            buffered: false, // For asynchronous logging
            shared: true) // If multiple processes log to the same file
        .Filter.ByExcluding(c =>
            c.Properties.Any(p => p.Value.ToString().Contains("SensitiveData"))) // Example of excluding sensitive data
);

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddSqlServerService(configuration);

builder.Services.AddJwtTokenService(configuration);

builder.Services.AddInjectService();

builder.Services.AddHealthCheckService();

builder.Services.AddControllers();

builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowAnyOrigin", corsPolicyBuilder =>
    {
        corsPolicyBuilder
            .SetIsOriginAllowed(x => _ = true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.InitializeRolesAsync();

app.UseHttpsRedirection();

app.UseCors("AllowAnyOrigin");

app.MapHealthChecks("/health/token", new HealthCheckOptions
{
    Predicate = check => check.Name == TokenServiceHealthCheck.HealthName
});
app.MapHealthChecks("/health/database", new HealthCheckOptions
{
    Predicate = check => check.Name == DatabaseHealthCheck<IAuthIdentityDbContext>.HealthName
});

app.UseMiddleware<TokenValidationMiddleware>();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseMiddleware<ValidationMappingMiddleware>();

app.MapControllers();

app.Run();
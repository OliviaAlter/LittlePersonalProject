using Identity.Health;
using Identity.Middleware;
using Identity.ServiceExtension;
using IdentityInfrastructure.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Configure Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/myapp.txt")); // Configure as needed

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

app.UseHttpsRedirection();

app.UseCors("AllowAnyOrigin");

app.MapHealthChecks("/health/token", new HealthCheckOptions
{
    Predicate = check => check.Name == TokenServiceHealthCheck.HealthName
});
app.MapHealthChecks("/health/database", new HealthCheckOptions
{
    Predicate = check => check.Name == DatabaseHealthCheck<IApplicationDbContext>.HealthName
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseMiddleware<ValidationMappingMiddleware>();

app.UseMiddleware<TokenValidationMiddleware>();

app.MapControllers();

app.Run();
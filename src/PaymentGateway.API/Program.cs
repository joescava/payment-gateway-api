using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using PaymentGateway.Infrastructure.MockAcquirer;
using PaymentGateway.Persistence.Repositories;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.Services;
using PaymentGateway.API.Middleware;
using PaymentGateway.API.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Include your API key in the header. Example: X-API-KEY: SuperSecretKey123",
        In = ParameterLocation.Header,
        Name = "X-API-KEY",
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddSingleton<IAcquirer, FakeAcquirer>();
builder.Services.AddSingleton<InMemoryPaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = ApiKeyAuthenticationHandler.SchemeName;
        options.DefaultChallengeScheme = ApiKeyAuthenticationHandler.SchemeName;
    })
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
        ApiKeyAuthenticationHandler.SchemeName, null);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
//using AuthFlowMaui.Shared.Settings;
//using AuthFlowMaui.Api.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using AuthFlowMaui.Api.Settings;
using Microsoft.AspNetCore.Authorization;

namespace AuthFlowMaui.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void AddJwtBearerConfig(this WebApplicationBuilder builder)
    {
        var jwtBearerConfig = builder.Configuration.GetSection("JwtBearer");
        builder.Services.Configure<JwtBearerConfig>(jwtBearerConfig);
    }
    public static void AddKeycloakAuthorization(this WebApplicationBuilder builder, JwtBearerConfig jwtBearerConfig)
    {
        IdentityModelEventSource.ShowPII = true;

        builder.Services
            .AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                
            })
            .AddJwtBearer(option =>
            {
                option.Authority = jwtBearerConfig.Authority;
                option.SaveToken = jwtBearerConfig.SaveToken;
                option.RequireHttpsMetadata = jwtBearerConfig.RequireHttpsMetadata;

                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtBearerConfig.ValidateIssuer,
                    ValidateAudience = jwtBearerConfig.ValidateAudience,
                    ValidAudience = jwtBearerConfig.ValidAudience,
                    ValidateLifetime = jwtBearerConfig.ValidateLifetime,
                    ValidateIssuerSigningKey = jwtBearerConfig.ValidateIssuerSigningKey,
                    ValidIssuer = jwtBearerConfig.ValidIssuer
                };

            });
        builder.Services.AddAuthorization(option =>
        {
            option.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                //.RequireClaim("email_verified", "true")
                .Build();           
        });
    }
}

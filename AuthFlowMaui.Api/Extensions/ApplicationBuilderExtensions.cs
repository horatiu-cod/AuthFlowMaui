using AuthFlowMaui.Shared.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using AuthFlowMaui.Api.Settings;

namespace AuthFlowMaui.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void AddJwtBearerConfig(this WebApplicationBuilder builder)
    {
        var jwtBearerConfig = builder.Configuration.GetSection("JwtBearer");
        builder.Services.Configure<JwtBearerConfig>(jwtBearerConfig);
    }
    public static void AddKeycloakAuthorization(this WebApplicationBuilder builder)
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
                option.Authority = "https://localhost:8843/realms/develop";// de verificat
                option.SaveToken = false;
                //RequireHttpsMetadata option was set to false only because the running environment is Development.
                //In Production, this option is by default set as true.
                option.RequireHttpsMetadata = false;

                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://localhost:8843/realms/dev"
                };

            });
    }
}

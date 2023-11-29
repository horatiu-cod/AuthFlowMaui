using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using AuthFlowMaui.MinimalApi.Settings;


namespace AuthFlowMaui.MinimalApi.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddJwtBearerConfig(this WebApplicationBuilder builder)
    {
        var jwtBearerConfig = builder.Configuration.GetSection("JwtBearer");
        if (jwtBearerConfig != null)
        {
            builder.Services.Configure<JwtBearerConfig>(jwtBearerConfig);
        }
        return builder;
    }
    public static WebApplicationBuilder AddKeycloakAuthorization(this WebApplicationBuilder builder)
    {
        IdentityModelEventSource.ShowPII = true;
        var jwtBearerConfig = builder.Configuration.GetSection("JwtBearer").Get<JwtBearerConfig>();
        if (jwtBearerConfig is not null)
        builder.Services
            .AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.Authority = jwtBearerConfig.Authority;
                option.SaveToken = jwtBearerConfig.SaveToken;
                option.RequireHttpsMetadata = jwtBearerConfig.RequireHttpsMetadata;

                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtBearerConfig.ValidIssuer,
                    ValidAudience = jwtBearerConfig.ValidAudience,
                    ValidateIssuer = jwtBearerConfig.ValidateIssuer,
                    ValidateAudience = jwtBearerConfig.ValidateAudience,
                    ValidateIssuerSigningKey = jwtBearerConfig.ValidateIssuerSigningKey,
                    ValidateLifetime = jwtBearerConfig.ValidateLifetime,
                };
            });
        //builder.Services.AddAuthorization(option =>
        //{
        //    option.DefaultPolicy = new AuthorizationPolicyBuilder()
        //        .RequireAuthenticatedUser()
        //        .RequireClaim("email_verified", "true")
        //        //.RequireClaim("scope", "aud_api_maui")
        //        .Build();
        //});
        builder.Services.AddAuthorization(option =>
        {
            option.AddPolicy("RequireUserRole", policy =>
                policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("email_verified", "true")
                    .RequireRole("user_role"));

        });
        return builder;
    }

}

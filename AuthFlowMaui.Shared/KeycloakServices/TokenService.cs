using AuthFlowMaui.Shared.KeycloakUtils;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthFlowMaui.Shared.KeycloakServices;

public class TokenService : ITokenService
{
    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameter = new TokenValidationParameters
        {
            ValidateLifetime = true,
        };
        try
        {
            return tokenHandler.ValidateToken(token, validationParameter, out _);
        }
        catch (Exception)
        {

            return null;
        }
    }
    public async Task<Result> ValidateTokenAsync(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameter = new TokenValidationParameters
        {
            ValidateIssuer = true,

#if ANDROID
            ValidIssuer = "https://10.0.2.2:8843/realms/dev",
#else
            ValidIssuer = "https://localhost:8843/realms/dev",
#endif
            ValidateAudience = false,
            ValidAudience = "demo-client",
            ValidateLifetime = true,
            ValidateIssuerSigningKey = false,
            SignatureValidator = delegate (string token, TokenValidationParameters parameters)
            {
                var jwt = new JwtSecurityToken(token);

                return jwt;
            }

        };
        try
        {
            var result = await tokenHandler.ValidateTokenAsync(token, validationParameter);
            if (result.IsValid)
            {
                return Result.Success();
            }
            else
            {
                return Result.Fail("");
            }
        }
        catch (Exception ex)
        {

            return Result.Fail($"{ex.Message}");

        }

    }
}

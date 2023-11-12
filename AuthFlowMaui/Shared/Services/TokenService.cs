using AuthFlowMaui.Shared.Utils;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthFlowMaui.Shared.Services;

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
    public async Task<MethodResult> ValidateTokenAsync(string token)
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
                return MethodResult.Success();
            }
            else
            {
                return MethodResult.Fail("");
            }
        }
        catch (Exception ex)
        {

            return MethodResult.Fail($"{ex.Message}");

        }

    }
}

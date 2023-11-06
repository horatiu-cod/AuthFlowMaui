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
            ValidateLifetime = true,
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

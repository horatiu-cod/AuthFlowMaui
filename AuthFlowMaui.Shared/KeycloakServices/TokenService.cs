using AuthFlowMaui.Shared.KeycloakUtils;
using AuthFlowMaui.Shared.TokenDtos;
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
    public async Task<Result> ValidateTokenAsync(string token, KeycloakTokenValidationParametersDto keycloakTokenValidationParametersDto)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameter = new TokenValidationParameters
        {
            ValidateIssuer = true,

            ValidIssuer = keycloakTokenValidationParametersDto.ValidIssuer,
            ValidateAudience = true,
            ValidAudiences = keycloakTokenValidationParametersDto.ValidAudiences,
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
    public async Task<Result> ValidateRefreshTokenAsync(string refreshTtoken, KeycloakTokenValidationParametersDto keycloakTokenValidationParametersDto)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameter = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = keycloakTokenValidationParametersDto.ValidIssuer,
            ValidateAudience = true,
            ValidAudience = keycloakTokenValidationParametersDto.ValidAudience,
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
            var result = await tokenHandler.ValidateTokenAsync(refreshTtoken, validationParameter);
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

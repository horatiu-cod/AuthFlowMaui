using AuthFlowMaui.Shared.KeycloakUtils;
using AuthFlowMaui.Shared.TokenDtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
namespace AuthFlowMaui.Shared.KeycloakServices;

public class KeycloakTokenValidationService : IKeycloakTokenValidationService
{
    public async Task<Result> ValidateTokenAsync(string token, KeycloakTokenValidationParametersDto keycloakTokenValidationParametersDto)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameter = new TokenValidationParameters
        {
            ValidateIssuer = true,
            IssuerSigningKey = keycloakTokenValidationParametersDto.IssuerSigningKey,
            ValidIssuer = keycloakTokenValidationParametersDto.ValidIssuer,
            ValidateAudience = true,
            ValidAudiences = keycloakTokenValidationParametersDto.ValidAudiences,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
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
                return Result.Fail($"{result.Exception.Message} Token validation inner Exception from ValidateTokenAsync");
            }
        }
        catch (Exception ex)
        {

            return Result.Fail($"{ex.Message} Token validation Exception from ValidateTokenAsync");

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
            ValidateIssuerSigningKey = true,
            //SignatureValidator = delegate (string token, TokenValidationParameters parameters)
            //{
            //    var jwt = new JwtSecurityToken(token);

            //    return jwt;
            //}
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
                return Result.Fail($"{result.Exception.Message} Token validation inner Exception from ValidateRefreshTokenAsync");
            }
        }
        catch (Exception ex)
        {

            return Result.Fail($"{ex.Message} Token validation Exception from ValidateRefreshTokenAsync");

        }

    }

}

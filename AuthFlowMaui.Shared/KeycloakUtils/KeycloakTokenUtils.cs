using AuthFlowMaui.Shared.Constants;
using AuthFlowMaui.Shared.TokenDtos;

namespace AuthFlowMaui.Shared.KeycloakUtils;

public static class KeycloakTokenUtils
{
    public static FormUrlEncodedContent GetUserTokenRequestBody(KeycloakUserTokenRequestDto tokenRequestDtos)
    {
        //to request token we need FormUrlEncodedContentType, we create a list of
        var keyValuePairs = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>(KeycloakAccessTokenConst.GrantType, tokenRequestDtos.GrantType),
            new KeyValuePair<string, string>(KeycloakAccessTokenConst.ClientId, tokenRequestDtos.ClientId),
            new KeyValuePair<string, string>(KeycloakAccessTokenConst.ClientSecret, tokenRequestDtos.ClientSecret),
            new KeyValuePair<string, string>(KeycloakAccessTokenConst.Username, tokenRequestDtos.Username),
            new KeyValuePair<string, string>(KeycloakAccessTokenConst.Password, tokenRequestDtos.Password)
        };
        return new FormUrlEncodedContent(keyValuePairs);
    }
    public static FormUrlEncodedContent GetClientTokenRequestBody(KeycloakClientRequestDto tokenRequestDto)
    {
        var keyValuePairs = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>(KeycloakAccessTokenConst.GrantType, tokenRequestDto.GrantType),
            new KeyValuePair<string, string>(KeycloakAccessTokenConst.ClientId, tokenRequestDto.ClientId),
            new KeyValuePair<string, string>(KeycloakAccessTokenConst.ClientSecret, tokenRequestDto.ClientSecret)

        };
        return new FormUrlEncodedContent(keyValuePairs);
    }
    public static FormUrlEncodedContent GetUserTokenWithRefreshTokenRequestBody(KeycloakUserTokenWithRefreshTokenRequestDto tokenRequestDto)
    {
        var keyValuePairs = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>(KeycloakAccessTokenConst.GrantType, tokenRequestDto.GrantType),
            new KeyValuePair<string, string>(KeycloakAccessTokenConst.ClientId, tokenRequestDto.ClientId),
            new KeyValuePair<string, string>(KeycloakAccessTokenConst.ClientSecret, tokenRequestDto.ClientSecret),
            new KeyValuePair<string, string>(KeycloakAccessTokenConst.RefreshToken, tokenRequestDto.RefreshToken)
        };
        return new FormUrlEncodedContent(keyValuePairs);
    }
}

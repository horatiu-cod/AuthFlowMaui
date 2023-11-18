using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakUtils;
using System.Net.Http.Headers;
using System.Text;

namespace AuthFlowMaui.Shared.KeycloakServices;

public class KeycloakApiService : IKeycloakApiService
{
    private IHttpClientFactory _httpClientFactory;

    public KeycloakApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Result> RegisterKeycloakUser(KeycloakRegisterUserDto keycloakRegisterUserDto, string token, string httpClientName, string userUrl, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient(httpClientName);
        var RegisterUserBody = keycloakRegisterUserDto.ToJson();
        var stringContent = new StringContent(RegisterUserBody, Encoding.UTF8, "application/json");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await httpClient.PostAsync(userUrl, stringContent, cancellationToken);

        if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Result.Fail("You are unauthorized");
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result.Fail($"{result.StatusCode} {result.ReasonPhrase}");
        }
        else
        {
            return Result.Success();
        }
    }
}

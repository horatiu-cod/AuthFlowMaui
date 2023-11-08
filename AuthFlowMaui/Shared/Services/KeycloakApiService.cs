using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.Utils;
using System.Net.Http.Headers;
using System.Text;

namespace AuthFlowMaui.Shared.Services;

public class KeycloakApiService : IKeycloakApiService
{
    private IHttpClientFactory _httpClientFactory;

    public KeycloakApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<MethodResult> RegisterKeycloakUser(KeycloakRegisterUserDto keycloakRegisterUserDto, string token)
    {
        var httpClient = _httpClientFactory.CreateClient("maui-to-https-keycloak");
        var RegisterUserBody = keycloakRegisterUserDto.ToJson();
        var stringContent = new StringContent(RegisterUserBody, Encoding.UTF8, "application/json");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await httpClient.PostAsync("/admin/realms/dev/users", stringContent);

        if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return MethodResult.Fail("You are unauthorized");
        }
        else if (!result.IsSuccessStatusCode)
        {
            return MethodResult.Fail($"{result.StatusCode} {result.ReasonPhrase}");
        }
        else
        {
            return MethodResult.Success();
        }
    }
}

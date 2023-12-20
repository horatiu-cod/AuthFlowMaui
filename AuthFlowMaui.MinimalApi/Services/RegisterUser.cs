using AuthFlowMaui.MinimalApi.Settings;
using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.Repositories;

namespace AuthFlowMaui.MinimalApi.Services;
#pragma warning disable
public class RegisterUser : IRegisterUser
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IApiRepository _repository;

    public RegisterUser(IConfiguration configuration, IHttpClientFactory httpClientFactory, IApiRepository apiRepository)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _repository = apiRepository;
    }

    public async Task<IResult> Register(RegisterUserDto keycloakRegisterUserDto, CancellationToken cancellationToken)
    {
        var authClient = _configuration.GetRequiredSection("AuthClientSettings").Get<AuthClientConfig>();
        var role = _configuration.GetRequiredSection("UserRole").Get<RoleSettings>();
        keycloakRegisterUserDto.RoleName = role.Name;
        var client = _configuration.GetRequiredSection("ClientSettings").Get<ClientConfig>();
        keycloakRegisterUserDto.ClientId = client.ClientId;

        var httpClient = _httpClientFactory.CreateClient(Constants.HttpClientName);

        var result = await _repository.Register(keycloakRegisterUserDto, authClient.ClientId, authClient.ClientSecret, authClient.Realm, httpClient, cancellationToken);
        if (!result.IsSuccess)
        {
            return TypedResults.Problem(result.Error, null, (int)result.StatusCode);
        }
        else
        {
            return TypedResults.Created("", result);
        }
    }
}

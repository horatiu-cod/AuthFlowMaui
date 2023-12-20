using AuthFlowMaui.MinimalApi;
using AuthFlowMaui.MinimalApi.Extensions;
using AuthFlowMaui.MinimalApi.Services;
using AuthFlowMaui.MinimalApi.Settings;
using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.Extensions;

#pragma warning disable
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient(Constants.HttpClientName, httpClient =>
{
    var settings = builder.Configuration.GetRequiredSection("ClientSettings").Get<AuthClientConfig>();
    var baseUrl = settings.BaseUrl;
    httpClient.BaseAddress = new Uri(baseUrl);
});
builder.AddKeycloakAuthorization();

builder.Services.ConfigureKeycloak();
builder.Services.AddScoped<IRegisterUser, RegisterUser>();

builder.AddKeycloakAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("api/user/authorization", () => "ok merge")
    .RequireAuthorization("RequireUserRole")
    .Produces(200)
    .Produces(401)
    .WithOpenApi();

app.MapPut("api/user/register", async (IHttpClientFactory _httpClientFactory, IApiRepository _repository, KeycloakRegisterUserDto keycloakRegisterUserDto, CancellationToken cancellationToken) =>
{
    var authClientConfig = builder.Configuration.GetRequiredSection("AuthClientSettings").Get<AuthClientConfig>();
    //TODO map KeycloakClientSettings with AuthClientConfig
    var clientSettings = new KeycloakClientSettings
    {
        ClientId = authClientConfig.ClientId,
        ClientSecret = authClientConfig.ClientSecret,
        Realm = authClientConfig.Realm,
        RealmUrl = authClientConfig.RealmUrl
    };

    var httpClient = _httpClientFactory.CreateClient("api-http_client");

    var signupResult = await _repository.RegisterKeycloakUser(keycloakRegisterUserDto, clientSettings, httpClient, cancellationToken);
    if (!signupResult.IsSuccess)
    {
        return Results.Problem(signupResult.Error, null, (int?)signupResult.StatusCode);
    }
    var getUserResult = await _repository.GetKeycloakUser(keycloakRegisterUserDto.UserName, clientSettings, httpClient, cancellationToken);
    if (!getUserResult.IsSuccess)
    {
        return Results.Problem(getUserResult.Error, null, (int?)getUserResult.StatusCode);
    }
    var clientConfig = builder.Configuration.GetRequiredSection("ClientSettings").Get<ClientConfig>();
    var role = builder.Configuration.GetRequiredSection("UserRole").Get<RoleSettings>();
    var roleDto = new KeycloakRoleDto
    {
        Id = role.Id,
        Name = role.Name
    };
    KeycloakRoleDto[] roleDtos = [roleDto];
    var asignResult = await _repository.AsignRoleToKeycloakUser(getUserResult.Content, clientSettings, roleDtos, httpClient, clientConfig.ClientUuID, cancellationToken);
    if (!asignResult.IsSuccess)
    {
        await _repository.DeletKeycloakUser(getUserResult.Content!, clientSettings, httpClient, cancellationToken);
        return Results.Problem(asignResult.Error, null, (int?)asignResult.StatusCode);
    }
    return Results.Created("", getUserResult);
}).WithName("registration");

app.Run();

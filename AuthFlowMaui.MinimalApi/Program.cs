using AuthFlowMaui.MinimalApi.Extensions;
using AuthFlowMaui.MinimalApi.Settings;
using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.Extensions;
using AuthFlowMaui.Shared.KeycloakSettings;
using AuthFlowMaui.Shared.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("api-http_client",httpClient =>
{
    var baseUrl = "https://localhost:8843";
    httpClient.BaseAddress = new Uri(baseUrl);
});
   
builder.Services.ConfigureKeycloak();

builder.AddJwtBearerConfig();
builder.AddClientSettingsConfig();
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
    var clientSettingsConfig = builder.Configuration.GetRequiredSection("ClientSettings").Get<AuthClientConfig>();
    //TODO map KeycloakClientSettings with AuthClientConfig
    var clientSettings = new KeycloakClientSettings
    {
        ClientId = clientSettingsConfig.ClientId,
        ClientSecret = clientSettingsConfig.ClientSecret,
        RealmUrl = clientSettingsConfig.RealmUrl,
        Realm = clientSettingsConfig.Realm
    };
    var httpClient = _httpClientFactory.CreateClient("api-http_client");
    var result = await _repository.RegisterKeycloakUser(keycloakRegisterUserDto, clientSettings, httpClient, cancellationToken);
    //var httpCode = result.HttpStatus;
    if (result.IsSuccess)
    {
        return Results.Created( "", result);
    }
    else
    {
        return Results.Problem(result.Error, null, (int?)result.HttpStatus);
    }
});

app.Run();

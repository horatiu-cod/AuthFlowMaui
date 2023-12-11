using AuthFlowMaui.MinimalApi.Extensions;
using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakServices;
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
   
builder.Services.AddScoped<IApiRepository, ApiRepository>();
builder.Services.AddScoped<IKeycloakTokenService, KeycloakTokenService>();

builder.AddJwtBearerConfig();
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
    var clientSettings = new KeycloakClientSettings
    {
        ClientId = "api-client",
        ClientSecret = "xrJ4rQG1UDbfpHGgqCeclkaSzedm8WQY",
        RealmUrl = "/realms/dev/protocol/openid-connect",
        Realm = "dev"
    };
    var httpClient = _httpClientFactory.CreateClient("api-http_client");
    var result = await _repository.RegisterKeycloakUser(keycloakRegisterUserDto, clientSettings, httpClient, cancellationToken);
    return result;
});

app.Run();

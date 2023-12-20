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
    var settings = builder.Configuration.GetRequiredSection("AuthClientSettings").Get<AuthClientConfig>();
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


app.MapPut("api/user/register", async (IRegisterUser register, RegisterUserDto keycloakRegisterUserDto, CancellationToken cancellationToken) =>
{
    var result = register.Register( keycloakRegisterUserDto, cancellationToken);
    return await result;
})
    .Produces<KeycloakUserDto>()
    .Produces(500)
    .Produces(200)
    .Produces(201)
    .Produces(409)
    .Produces(401)
    .WithName("register");


app.Run();

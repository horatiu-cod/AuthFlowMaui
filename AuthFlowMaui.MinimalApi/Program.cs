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

app.Run();

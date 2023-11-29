using AuthFlowMaui.MinimalApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

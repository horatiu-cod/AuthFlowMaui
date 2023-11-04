using AuthFlowMaui.Api.Extensions;
using AuthFlowMaui.Api.Settings;
//using AuthFlowMaui.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//builder.Services.AddServices();
builder.AddJwtBearerConfig();
var jwtBearerConfig =  builder.Configuration.GetSection("JwtBearer").Get<JwtBearerConfig>();

builder.AddKeycloakAuthorization(jwtBearerConfig!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

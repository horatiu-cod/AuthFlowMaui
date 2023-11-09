using AuthFlowMaui.Shared.Services;
using AuthFlowMaui.Shared.Settings;
using System.IdentityModel.Tokens.Jwt;

namespace AuthFlowMaui.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly IKeycloakTokenService _keycloakTokenService;
        private readonly IStorageService _storageService;
        public MainPage(IKeycloakTokenService keycloakTokenService, IStorageService storageService)
        {
            InitializeComponent();
            _keycloakTokenService = keycloakTokenService;
            _storageService = storageService;
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            var keycloakSettings = new KeycloakSettings();
            var clientSettings = await _storageService.GetClientSecretAsync();
            if (clientSettings.IsSuccess) 
            {
                keycloakSettings = keycloakSettings.FromJson(clientSettings.Data);
                try
                {
                    var response = await _keycloakTokenService.GetClientTokenResponseAsync(keycloakSettings);
                    if (response.IsSuccess)
                    {
                        var rawToken = response.Data.AccessToken;
                        //var userName = await response.Data.HttpResponse.Content.ReadAsStringAsync();
                        var handler = new JwtSecurityTokenHandler();
                        var token = handler.ReadJwtToken(rawToken);
                        var claim = token.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
                        //var res = response.;
                        apiResponse.Text = $"{claim} merge";
                    }
                    else
                    {
                        apiResponse.Text = response.Error;
                    }

                }
                catch (Exception ex)
                {
                    apiResponse.Text = ex.Message;
                    throw;
                };


            }

        }
    }

}

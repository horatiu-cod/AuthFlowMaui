using AuthFlowMaui.Shared.KeycloakServices;
using AuthFlowMaui.Shared.Services;
using System.IdentityModel.Tokens.Jwt;

namespace AuthFlowMaui.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly IKeycloakTokenService _keycloakTokenService;
        private readonly IStorageService _storageService;
        static readonly CancellationTokenSource s_tokenSource = new CancellationTokenSource();
        public MainPage(IKeycloakTokenService keycloakTokenService, IStorageService storageService)
        {
            InitializeComponent();
            _keycloakTokenService = keycloakTokenService;
            _storageService = storageService;
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            var httpClientName = RealmConstants.HttpClientName;
            var clientSettings = await _storageService.GetClientSecretAsync();
            if (clientSettings.IsSuccess) 
            {
                var keycloakSettings = clientSettings.Data;
                keycloakSettings.PostUrl = RealmConstants.RealmUrl;
                try
                {
                    s_tokenSource.CancelAfter(3500);
                    var response = await _keycloakTokenService.GetClientTokenResponseAsync(keycloakSettings, httpClientName, s_tokenSource.Token);
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
                catch (Exception ex) when (ex is OperationCanceledException)
                {
                    apiResponse.Text = ex.Message;
                }
                finally
                {
                    s_tokenSource.Dispose();
                };


            }

        }
    }

}

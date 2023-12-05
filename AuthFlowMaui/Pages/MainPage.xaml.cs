using AuthFlowMaui.Constants;
using AuthFlowMaui.Shared.ClientHttpExtensions;
using AuthFlowMaui.Shared.Services;
using System.IdentityModel.Tokens.Jwt;

namespace AuthFlowMaui.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IStorageService _storageService;
        static readonly CancellationTokenSource s_tokenSource = new CancellationTokenSource();
        public MainPage(IStorageService storageService, IHttpClientFactory httpClientFactory)
        {
            InitializeComponent();
            _storageService = storageService;
            _httpClientFactory = httpClientFactory;
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            var httpClientName = RealmConstants.HttpClientName;
            var httpClient = _httpClientFactory.CreateClient(httpClientName);
            var clientSettings = await _storageService.GetClientSecretAsync();
            if (clientSettings.IsSuccess) 
            {
                var keycloakSettings = clientSettings.Data;
                keycloakSettings.RealmUrl = RealmConstants.RealmUrl;
                try
                {
                    s_tokenSource.CancelAfter(13500);
                    var response = await httpClient.GetClientTokenAsync(keycloakSettings, s_tokenSource.Token);
                    s_tokenSource.TryReset();
                    if (response.IsSuccess)
                    {
                        var rawToken = response.Content.AccessToken;
                        var handler = new JwtSecurityTokenHandler();
                        var token = handler.ReadJwtToken(rawToken);
                        var claim = token.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
                        apiResponse.Text = $"{claim} merge";
                    }
                    else
                    {
                        apiResponse.Text = $"{ response.Error} passed from GetClientTokenAsync to OnCounterClicked in MainPage.xaml.cs";
                    }

                }
                catch (Exception ex) when (ex is OperationCanceledException)
                {
                    apiResponse.Text = $"{ex.Message} OperationCanceledException from GetClientTokenAsync to OnCounterClicked in MainPage.xaml.cs";
                }
            }
        }
    }

}

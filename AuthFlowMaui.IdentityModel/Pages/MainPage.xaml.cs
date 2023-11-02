using IdentityModel.Client;
using System.IdentityModel.Tokens.Jwt;

namespace AuthFlowMaui.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ClientCredentialsTokenRequest _clientCredentialsTokenRequest;
        public MainPage(IHttpClientFactory httpClientFactory, ClientCredentialsTokenRequest clientCredentialsTokenRequest)
        {
            InitializeComponent();
            _httpClientFactory = httpClientFactory;
            _clientCredentialsTokenRequest = clientCredentialsTokenRequest;
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            using (var httpClient = _httpClientFactory.CreateClient("maui-to-https-keycloak"))
            {
                var response = await httpClient.RequestClientCredentialsTokenAsync(_clientCredentialsTokenRequest);

                if (!response.IsError)
                {
                    var rawToken = response.AccessToken;
                    var userName = await response.HttpResponse.Content.ReadAsStringAsync();
                    var handler = new JwtSecurityTokenHandler();
                    var token = handler.ReadJwtToken(rawToken);
                    var claim = token.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
                    var res = response.HttpStatusCode;
                    apiResponse.Text = res.ToString();
                }
            };
        }
    }

}

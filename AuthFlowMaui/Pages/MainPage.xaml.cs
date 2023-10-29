using IdentityModel.Client;

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
            var httpClient = _httpClientFactory.CreateClient("maui-to-https-keycloak");
            var response = await httpClient.RequestClientCredentialsTokenAsync(_clientCredentialsTokenRequest);

            if (!response.IsError)
            {
                var data = response.HttpResponse.Content.ReadAsStringAsync().Result;
                apiResponse.Text = data;
            }
        }
    }

}

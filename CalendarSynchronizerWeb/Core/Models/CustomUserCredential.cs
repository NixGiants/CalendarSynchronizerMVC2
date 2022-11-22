using Google.Apis.Http;
using System.Net.Http.Headers;


namespace Core.Models
{
    public class CustomUserCredential : IHttpExecuteInterceptor, IConfigurableHttpClientInitializer
    {
        private string _accessToken;

        public CustomUserCredential(string accessToken)
        {
            _accessToken = accessToken;
        }

        public void Initialize(ConfigurableHttpClient httpClient)
        {
            httpClient.MessageHandler.ExecuteInterceptors.Add(this);
        }

        public async Task InterceptAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }
    }
}
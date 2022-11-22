using Google.Apis.Http;
using System.Net.Http.Headers;

namespace CalendarSynchronizerWeb.Models
{
    
    public class CustomUserCredential : IHttpExecuteInterceptor, IConfigurableHttpClientInitializer
    {
        private readonly string accessToken;

        public CustomUserCredential(string accessToken)
        {
            this.accessToken = accessToken;
        }

        public void Initialize(ConfigurableHttpClient httpClient)
        {
            httpClient.MessageHandler.ExecuteInterceptors.Add(this);
        }

        public async Task InterceptAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}

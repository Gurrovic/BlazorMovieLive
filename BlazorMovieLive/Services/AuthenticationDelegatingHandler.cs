using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace BlazorMovieLive.Client.Services
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;

        public AuthenticationDelegatingHandler(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrWhiteSpace(savedToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}

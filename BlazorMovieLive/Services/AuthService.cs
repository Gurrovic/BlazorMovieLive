using Blazored.LocalStorage;
using BlazorMovieLive.Client.Services.Contracts;
using BlazorMovieLive.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using BlazorMovieLive.Client.Utility;
using BlazorMovieLive.Client.Models;

namespace BlazorMovieLive.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(IHttpClientFactory httpClientFactory,
                            AuthenticationStateProvider authenticationStateProvider,
                            ILocalStorageService localStorage)
        {
            _httpClient = httpClientFactory.CreateClient("InternalApi");
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;            
        }       

        public async Task<RegisterResult> Register(RegisterModel registerModel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/accounts", registerModel);
            if (result.IsSuccessStatusCode)
                return new RegisterResult { Successful = true, Errors = null };
            return new RegisterResult { Successful = false, Errors = new List<string> { "Error occured" } };
        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            var loginAsJson = JsonSerializer.Serialize(loginModel);

            var response = await _httpClient.PostAsync("api/Login", 
                new StringContent(loginAsJson, Encoding.UTF8, "application/json"));

            var loginResult = JsonSerializer.Deserialize<LoginResult>(await response.Content.ReadAsStringAsync(), 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });           

            if (!response.IsSuccessStatusCode)
            {
                return loginResult!;
            }

            await _localStorage.SetItemAsync("authToken", loginResult!.Token);
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email!);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);

            return loginResult;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<UserSettingsModel> GetUserSettings()
        {
            var response = await _httpClient.GetFromJsonAsync<UserSettingsModel>("api/accounts/getuserinfo");            
            return response ?? new UserSettingsModel();
        }

        public async Task<bool> UpdateUserSettings(UserSettingsModel settingsModel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/accounts/updateuserinfo", settingsModel);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> AddToFavorites(FavoriteMovieDto favoriteMovieDto)
        {           
            var response = await _httpClient.PostAsJsonAsync("api/favorites", favoriteMovieDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<int>> GetFavoriteMovieIdsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<int>>("api/favorites");
                return response ?? new List<int>(); // Return an empty list if the response is null
            }
            catch (Exception ex)
            {
                // Log the exception and handle it as needed
                // For simplicity, returning an empty list here
                return new List<int>();
            }
        }
    }
}

using BlazorMovieLive.Client.Models;
using BlazorMovieLive.Shared.Models;

namespace BlazorMovieLive.Client.Services.Contracts
{
    public interface IAuthService
    {
        Task<RegisterResult> Register(RegisterModel registerModel);
        Task<LoginResult> Login(LoginModel loginModel);
        Task Logout();
        Task<UserSettingsModel> GetUserSettings();
        Task<bool> UpdateUserSettings(UserSettingsModel settingsModel);
        Task<bool> AddToFavorites(FavoriteMovieDto favoriteMovieDto);
        Task<List<int>> GetFavoriteMovieIdsAsync();
        Task<bool> RemoveFromFavorites(int id);
    }
}

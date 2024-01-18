using BlazorMovieLive.Client.Models;
using BlazorMovieLive.Shared.Models;

namespace BlazorMovieLive.Client.Services.Contracts
{
    public interface IAuthService
    {
        Task<RegisterResult> Register(RegisterModel registerModel);
        Task<LoginResult> Login(LoginModel loginModel);
        Task Logout();
        Task<UserSettingsModelDto> GetUserSettings();
        Task<bool> UpdateUserSettings(UserSettingsModelDto settingsModel);
        Task<bool> AddToFavorites(FavoriteMovieDto favoriteMovieDto);
        Task<List<int>> GetFavoriteMovieIdsAsync();
        Task<bool> RemoveFromFavorites(int id);
    }
}

namespace BlazorMovieLive.Client.Models
{
    public class UserSettingsModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; } = string.Empty; // Default value if null
        public string LastName { get; set; } = string.Empty;  // Default value if null
    }
}

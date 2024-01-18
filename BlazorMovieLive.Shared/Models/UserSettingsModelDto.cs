namespace BlazorMovieLive.Client.Models
{
    public class UserSettingsModelDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; } = string.Empty; // Default value if null
        public string LastName { get; set; } = string.Empty;  // Default value if null
        public string PhoneNumber { get; set; } = string.Empty; // Default value if null
    }
}

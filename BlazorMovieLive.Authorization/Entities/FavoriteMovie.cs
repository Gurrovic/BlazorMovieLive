using BlazorMovieLive.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovieLive.Server.Entities
{
    public class FavoriteMovie
    {        
        public int FavoriteMovieId { get; set; } // Primary key

        // Foreign key for User (assuming you have a User entity for identity)
        public string UserId { get; set; }
        public ApplicationUser User { get; set; } // Navigation property

        // Movie information
        public int MovieId { get; set; } // ID from the external API
        public string MovieTitle { get; set; } // You could store additional movie details if needed
    }
}


using BlazorMovieLive.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovieLive.Server.Entities
{
    public class FavoriteMovie
    {        
        public int FavoriteMovieId { get; set; } // Primary key        
        public string UserId { get; set; } // Foreign key for User (assuming you have a User entity for identity)
        public ApplicationUser User { get; set; } // Navigation property        
        public int MovieId { get; set; } // ID from the external API
        public string MovieTitle { get; set; } // You could store additional movie details if needed
    }
}


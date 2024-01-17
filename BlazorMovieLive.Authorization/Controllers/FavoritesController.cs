using BlazorMovieLive.Authorization.Data;
using BlazorMovieLive.Server.Data;
using BlazorMovieLive.Server.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlazorMovieLive.Shared.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using BlazorMovieLive.Client.Pages;

namespace BlazorMovieLive.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly BlazorMovieLiveDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoritesController(BlazorMovieLiveDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost]        
        public async Task<IActionResult> AddToFavorites([FromBody] FavoriteMovieDto favoriteMovieDto)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            var favoriteMovie = new FavoriteMovie
            {
                UserId = userId,
                MovieId = favoriteMovieDto.MovieId,
                MovieTitle = favoriteMovieDto.MovieTitle
            };

            _context.Favorites.Add(favoriteMovie);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        [HttpGet]             
        public async Task<IActionResult> GetUserFavorites()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            var favoriteMoviesIds = await _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.MovieId)
                .ToListAsync();

            return Ok(favoriteMoviesIds);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavorite(int id)
        {
            var userId = _userManager.GetUserId(User); // Get the user's ID

            // Find the favorite movie in the database
            var favorite = await _context.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.MovieId == id);

            if (favorite != null)
            {                
                _context.Favorites.Remove(favorite);                
                await _context.SaveChangesAsync();

                return Ok(); // Return a success response
            }
            else
            {
                return NotFound(); // Favorite movie not found
            }
        }
    }
}

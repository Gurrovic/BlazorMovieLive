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

        [HttpPost]        
        public async Task<IActionResult> AddToFavorites([FromBody] FavoriteMovieDto favoriteMovieDto)
        {
            if (User == null)
            {
                Console.WriteLine("User object is null");
                return Unauthorized();
            }

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return Unauthorized();
            }

            var favoriteMovie = new FavoriteMovie
            {
                UserId = user.Id,
                MovieId = favoriteMovieDto.MovieId,
                MovieTitle = favoriteMovieDto.MovieTitle
            };

            _context.Favorites.Add(favoriteMovie);
            await _context.SaveChangesAsync();

            return Ok();
        }

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
    }
}

using BlazorMovieLive.Client.Models;
using BlazorMovieLive.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlazorMovieLive.Server.Data;
using System.Security.Claims;

namespace BlazorMovieLive.Authorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountsController(UserManager<ApplicationUser> userManager)
        {
            _userManager= userManager;            
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterModel model)
        {
            var newUser = new ApplicationUser 
            { 
                UserName = model.Email, 
                Email = model.Email, 
                FirstName = model.FirstName, 
                LastName = model.LastName 
            };

            var result = await _userManager.CreateAsync(newUser, model.Password!);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                return Ok(new RegisterResult { Successful = false, Errors = errors });
            }

            return Ok(new RegisterResult { Successful = true });
        }
        [Authorize]
        [HttpGet("getuserinfo")]
        public async Task<ActionResult<UserSettingsModel>> GetUserInfo()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("User ID is null or empty");
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }     

            var model = new UserSettingsModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return model;
        }

        [Authorize]
        [HttpPost("updateuserinfo")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserSettingsModel model)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                return NotFound();
            }

            // Update user properties
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            // Handle the update
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }

            // If the update fails, return the errors
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new { Errors = errors });
        }
    }
}

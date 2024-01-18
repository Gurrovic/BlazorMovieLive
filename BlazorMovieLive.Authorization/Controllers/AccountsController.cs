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

            // Generate an email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);            
            var test = await _userManager.ConfirmEmailAsync(newUser, token);
            Console.WriteLine("HEJSAN");

            // Create a confirmation link with the token
            //var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = newUser.Id, code = token }, protocol: HttpContext.Request.Scheme);

            //// For testing purposes, you can display the confirmation link or log it
            //Console.WriteLine($"Confirmation Link: {confirmationLink}");            

            return Ok(new RegisterResult { Successful = true });
        }

        [Authorize]
        [HttpGet("getuserinfo")]
        public async Task<ActionResult<UserSettingsModelDto>> GetUserInfo()
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

            var model = new UserSettingsModelDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            };

            return model;
        }

        [Authorize]
        [HttpPost("updateuserinfo")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserSettingsModelDto model)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            // Update user properties
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;

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

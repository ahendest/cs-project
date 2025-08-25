using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using cs_project.Core.DTOs;

namespace cs_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, ILogger<AccountController> logger) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
                return Ok("User registered.");
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null) return Unauthorized("Invalid credentials.");

            var result = await signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: true);
            if (result.IsLockedOut) return BadRequest("Account is temporarily locked. Try again later.");
            if (!result.Succeeded) return Unauthorized("Invalid credentials.");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                configuration["Jwt:Key"] ?? throw new Exception("JWT key not configured")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            logger.LogInformation("Issuer: {Issuer}", configuration["Jwt:Issuer"]);
            logger.LogInformation("Audience: {Audience}", configuration["Jwt:Audience"]);

            return Ok(new { token = tokenString });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserProfileDTO>> GetCurrentUser()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            return Ok(new UserProfileDTO
            {
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty
            });
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<UserProfileDTO>> UpdateProfile([FromBody] UserProfileUpdateDTO updateDto)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(updateDto.UserName)) user.UserName = updateDto.UserName;
            if (!string.IsNullOrWhiteSpace(updateDto.Email)) user.Email = updateDto.Email;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new UserProfileDTO
            {
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty
            });
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return NoContent();
        }

    }
}

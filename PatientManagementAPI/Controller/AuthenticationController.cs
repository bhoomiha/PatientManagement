using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PatientManagementAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PatientManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

         [HttpPost("register")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> Register([FromBody] RegisterDoctor registerDoctor)
        {
            // Check if the user already exists
            var existingUser = await _userManager.FindByEmailAsync(registerDoctor.Email);
            if (existingUser != null)
            {
                return BadRequest("Doctor with this email already exists.");
            }

            // Create the user dynamically using the request data
            var user = new ApplicationUser
            {
                UserName = registerDoctor.UserName,
                Email = registerDoctor.Email
            };

            // Create the user with a default password
            var result = await _userManager.CreateAsync(user, registerDoctor.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Assign the "Doctor" role to the user
            await _userManager.AddToRoleAsync(user, "Doctor");

            return Ok("Doctor registered successfully.");
        }
    


[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
{
    var user = await _userManager.FindByNameAsync(loginRequest.Username);
    if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
        return Unauthorized("Invalid username or password.");

    var roles = await _userManager.GetRolesAsync(user);
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email)
    };
    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        expires: DateTime.UtcNow.AddHours(3),
        claims: claims,
        signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
            SecurityAlgorithms.HmacSha256));

    return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
}
    }
}

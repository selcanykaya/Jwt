using Jwt.Context;
using Jwt.Dtos;
using Jwt.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtDbContext _context;
        private readonly IConfiguration _configuration;
       public AuthController(JwtDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

            if(existingUser != null)
                return BadRequest("User already exists with this email");

            //New user
            var newUser = new UserEntity
            {
                Email = model.Email,
                Password = model.Password 
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }
            var token = Helper.JwtToken(user.Email, _configuration["Jwt:Key"], _configuration["Jwt:Issuer"], _configuration["Jwt:Audience"]);


            return Ok(new { token });
        }
    }
}

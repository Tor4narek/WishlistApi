using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services.Presenters;
using Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wishlist.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserPresenter _userPresenter;
        private readonly IConfiguration _configuration;

        public UserController(IUserPresenter userPresenter, IConfiguration configuration)
        {
            _userPresenter = userPresenter;
            _configuration = configuration;
        }

        // Регистрация пользователя
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel model, CancellationToken token)
        {
            if (model == null)
            {
                return BadRequest("Invalid registration data.");
            }

            try
            {
                await _userPresenter.CreateUserAsync(model.Name, model.Email, model.Password, token);
                return Ok(new { message = "User successfully registered" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Вход пользователя с генерацией JWT токена
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model, CancellationToken token)
        {
            if (model == null)
            {
                return BadRequest("Invalid login data.");
            }

            try
            {
                // Аутентификация пользователя
                var user = await _userPresenter.AuthenticateUserAsync(model.Email, model.Password, token);

                // Генерация JWT токена
                var tokenString = GenerateJwtToken(user);

                return Ok(new
                {
                    message = "User successfully authenticated",
                    token = tokenString,
                    user
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        // Пример защищённого маршрута для получения данных пользователя по ID
        [HttpGet("{userId}")]
        [Authorize] // Только авторизованные пользователи могут вызвать этот метод
        public async Task<IActionResult> GetUser(string userId, CancellationToken token)
        {
            try
            {
                var user = await _userPresenter.LoadUserAsync(userId, token);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Выход пользователя (опционально можно добавить логику для аннулирования токена)
        [HttpPost("logout")]
        [Authorize] // Защищённый маршрут
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _userPresenter.LogoutAsync();
                return Ok(new { message = "User logged out successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Метод для генерации JWT токена
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SKLAD.Dto;
using SKLAD.Entities;
using SKLAD.Services;

namespace SKLAD.Controllers
{
    // кароче долго думал как сделать решил через Identity по гайдам индуса с ютуба спасибоооооо
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthService _authService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            AuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // не знаю как правильно сделать с ролями, хотел по твоему видосу, но сложно для меня показалось
            await _userManager.AddToRoleAsync(user, "User");

            return Ok(new { Message = "зарегистрирован" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("нет пользователя");

            var isValidPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isValidPassword)
                return Unauthorized("пароль инвалид");

            var token = await _authService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }
    }
}

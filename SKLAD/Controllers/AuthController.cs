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
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            var user = await _authService.Register(dto);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var token = await _authService.Login(dto);
            return Ok(new { Token = token });
        }
    }
}

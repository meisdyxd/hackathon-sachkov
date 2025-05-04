using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SKLAD.BackgroundServices;
using SKLAD.Database;
using SKLAD.Services;

namespace SKLAD.Controllers
{
    // здесь кароче я думал сделать админку, ноооооо, а нужна? по типу внеплановая отработка инвентаризации.
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController: ControllerBase
    {
        private readonly AdminService _adminService;
        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("inventory/start")]
        public async Task<IActionResult> StartInventory()
        {
            CancellationTokenSource token = new CancellationTokenSource();
            await _adminService.CheckDiscrepancies(token.Token);
            return Ok("Инвентаризация запущена");
        }
    }
}

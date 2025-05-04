using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SKLAD.BackgroundServices;
using SKLAD.Database;

namespace SKLAD.Controllers
{
    // здесь кароче я думал сделать админку, ноооооо, а нужна? по типу внеплановая отработка инвентаризации.
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController: ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        public AdminController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("inventory/start")]
        public async Task<IActionResult> StartInventory()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WarehouseDbContext>();

            await new InventoryService(_serviceProvider, null!)
                .CheckDiscrepancies(context, CancellationToken.None);

            return Ok("Инвентаризация запущена");
        }
    }
}

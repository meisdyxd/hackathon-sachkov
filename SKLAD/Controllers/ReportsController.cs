using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKLAD.Database;
using SKLAD.Dto;
using SKLAD.Services;

namespace SKLAD.Controllers
{
    // ну тут типо отправить запрос на пополнение
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportsService _reportsService;

        public ReportsController(ReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        [HttpGet("stock")]
        public async Task<IActionResult> GetStockReport()
        {
            return Ok(await _reportsService.GetStockReportAsync());
        }

        [HttpGet("expirations")] // товары которые истекут в сроке действия в указанном диапозоне
        public async Task<IActionResult> GetExpiringProducts([FromQuery] int daysThreshold = 30)
        {
            return Ok(await _reportsService.GetExpiringProductsAsync(daysThreshold));
        }
    }
}

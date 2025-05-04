using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKLAD.Database;
using SKLAD.Dto;

namespace SKLAD.Controllers
{
    // ну тут типо отправить запрос на пополнение
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly WarehouseDbContext _context;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(
            WarehouseDbContext context,
            ILogger<ReportsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("stock")]
        public async Task<IActionResult> GetStockReport()
        {
            try
            {
                var report = await _context.Products
                    .Include(p => p.StorageZone)
                    .Select(p => new StockReportDto(
                        p.Id,
                        p.Name,
                        p.SKU,
                        p.StorageZone.Name,
                        p.Quantity,
                        p.StorageZone.Capacity - p.StorageZone.CurrentStock
                    ))
                    .AsNoTracking()
                    .ToListAsync();

                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "не генерируетсяяяяя");
                return StatusCode(500, "адьос, полегло");
            }
        }

        [HttpGet("expirations")] // товары которые истекут в сроке действия в указанном диапозоне
        public async Task<IActionResult> GetExpiringProducts(
            [FromQuery] int daysThreshold = 30)
        {
            try
            {
                var thresholdDate = DateTime.UtcNow.AddDays(daysThreshold);

                var products = await _context.Products
                    .Where(p => p.ExpiryDate <= thresholdDate)
                    .OrderBy(p => p.ExpiryDate)
                    .Select(p => new ExpirationReportDto(
                        p.Id,
                        p.Name,
                        p.SKU,
                        p.ExpiryDate,
                        (p.ExpiryDate - DateTime.UtcNow).Days
                    ))
                    .AsNoTracking()
                    .ToListAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "не генерируетсяяяяя");
                return StatusCode(500, "адьос, полегло");
            }
        }
    }
}

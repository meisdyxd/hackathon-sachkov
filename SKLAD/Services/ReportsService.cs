using Microsoft.EntityFrameworkCore;
using SKLAD.Controllers;
using SKLAD.Database;
using SKLAD.Dto;
using SKLAD.Entities;

namespace SKLAD.Services
{
    public class ReportsService
    {
        private readonly WarehouseDbContext _context;
        private readonly ILogger<ReportsController> _logger;
        public ReportsService(
            WarehouseDbContext context,
            ILogger<ReportsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<StockReportDto>> GetStockReportAsync()
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

                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "не генерируетсяяяяя");
                throw new Exception("адьос, полегло");
            }
        }
        public async Task<List<ExpirationReportDto>> GetExpiringProductsAsync(int daysThreshold)
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

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "не генерируетсяяяяя");
                throw new Exception("адьос, полегло");
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using SKLAD.Database;

namespace SKLAD.BackgroundServices
{
    public class InventoryService : BackgroundService
    {
        private readonly ILogger<InventoryService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public InventoryService(
            IServiceProvider serviceProvider,
            ILogger<InventoryService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            _logger.LogInformation("Inventory Service started");

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider
                            .GetRequiredService<WarehouseDbContext>();
                        await SyncSystemQuantities(context); // синхронизирует количество предметов системно и по настоящему, но что то оно не работает
                        await CheckDiscrepancies(context, ct); // ищет расхождения, работает вроде гуд
                    }

                    await Task.Delay(TimeSpan.FromHours(24), ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in inventory service");
                }
            }
        }

        public async Task CheckDiscrepancies(
            WarehouseDbContext context,
            CancellationToken ct)
        {
            var discrepancies = await context.Products
                .Where(p => p.Quantity != p.SystemQuantity)
                .ToListAsync(ct);

            foreach (var product in discrepancies)
            {
                _logger.LogWarning("Расхождение по товару {SKU} Система: {SystemQuantity}, Факт: {Quantity}", product.SKU, product.SystemQuantity, product.Quantity);

                // можно кароче админу там отправлять или кому то еще, ток хз как на почту это сделать отсюда
            }
        }
        private async Task SyncSystemQuantities(WarehouseDbContext context)
        {
            await context.Database.ExecuteSqlRawAsync("""
                UPDATE products 
                SET system_quantity = physical_quantity
                WHERE last_updated < NOW() - INTERVAL '1 hour'
                """);
            // не работает, вроде по логике написан правильно sql. но хз
        }
    }
}

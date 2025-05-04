using SKLAD.BackgroundServices;
using SKLAD.Database;

namespace SKLAD.Services
{
    public class AdminService
    {
        private readonly IServiceProvider _serviceProvider;
        public AdminService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task CheckDiscrepancies(CancellationToken token)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WarehouseDbContext>();
            await new InventoryService(_serviceProvider, null!)
                .CheckDiscrepancies(context, token);
        }
    }
}

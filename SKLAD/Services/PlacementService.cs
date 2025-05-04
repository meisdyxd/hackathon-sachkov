using Microsoft.EntityFrameworkCore;
using SKLAD.Database;

namespace SKLAD.Services
{
    public class PlacementService
    {
        private readonly WarehouseDbContext _context;

        public PlacementService(WarehouseDbContext context)
            => _context = context;

        public async Task<Guid> FindOptimalZone(string productType)
        {
            // ищем первую зону с подходящим типом и свободным местом, что то сложнее по алгоритму я не придумал, подключать нейросеть?
            var zone = await _context.StorageZones
                .Where(z => z.Type == productType)
                .OrderBy(z => z.CurrentStock / (double)z.Capacity)
                .FirstOrDefaultAsync();

            return zone?.Id ?? throw new Exception("Нет подходящей зоны");
        }
    }
}

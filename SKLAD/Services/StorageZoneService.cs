using Microsoft.EntityFrameworkCore;
using SKLAD.Database;
using SKLAD.Dto;
using SKLAD.Entities;

namespace SKLAD.Services
{
    // круд по работе с хранилищем зоны
    public class StorageZoneService
    {
        private readonly WarehouseDbContext _context;
        public StorageZoneService(WarehouseDbContext context)
        {
            _context = context;
        }
        public async Task<List<StorageZone>> GetAsync()
        {
            return await _context.StorageZones.ToListAsync();
        }
        public async Task<StorageZone> CreateAsync(StorageZoneCreateDto zoneDto)
        {
            var zone = new StorageZone()
            {
                Id = Guid.NewGuid(),
                Name = zoneDto.Name,
                Type = zoneDto.Type,
                Capacity = zoneDto.Capacity,
                CurrentStock = zoneDto.CurrentStock,
                WarehouseId = zoneDto.WarehouseId
            };
            await _context.StorageZones.AddAsync(zone);
            await _context.SaveChangesAsync();
            return zone;
        }
        public async Task<bool> CheckCapacity(Guid id, int requiredSpace)
        {
            var zone = await _context.StorageZones.SingleOrDefaultAsync(z => z.Id == id);
            if (zone == null)
            {
                throw new Exception("Зона не найдена");
            }
            if(zone.Capacity < requiredSpace)
            {
                return false;
            }
            return true;
        }
    }
}

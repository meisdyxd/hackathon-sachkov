using Microsoft.EntityFrameworkCore;
using SKLAD.Database;
using SKLAD.Dto;
using SKLAD.Entities;

namespace SKLAD.Services
{
    // круд по работе со складом
    public class WarehouseService
    {
        private readonly WarehouseDbContext _context;
        public WarehouseService(WarehouseDbContext context)
        {
            _context = context;
        }
        public async Task<List<WarehouseResponse>> GetAsync() // это пиздец, я пока убирал цикл, чуть не умер, храни меня господь и еф кор
        {
            return await _context.Warehouses.Include(x => x.Zones)
                .Select(x => new WarehouseResponse(x.Id, x.Name, x.Address, x.Zones
                .Select(y => new StorageZoneResponseDto(y.Id, y.Name, y.Type, y.Capacity, y.CurrentStock, y.XCoordinate, y.YCoordinate, y.Rack, y.Shelf, y.WarehouseId)).ToList())).ToListAsync();
        }
        public async Task<Warehouse> CreateAsync(WarehouseCreateDto warehouseCreateDto)
        {
            var warehouse = new Warehouse
            {
                Id = Guid.NewGuid(),
                Name = warehouseCreateDto.Name,
                Address = warehouseCreateDto.Address,
            };
            await _context.Warehouses.AddAsync(warehouse);
            await _context.SaveChangesAsync();
            return warehouse;
        }
        public async Task<Warehouse> UpdateAsync(Guid id, WarehouseUpdateDto warehouseUpdateDto) // кстати дай фидбек, так норм обновлять или не? я видел 
            // у тебя вроде в ролике про ExecuteUpdateAsync, что когда юзать?
        {
            var warehouse = await _context.Warehouses.SingleOrDefaultAsync(w => w.Id == id);
            if (warehouse is null) throw new ArgumentNullException(nameof(warehouse));
            warehouse.Address = warehouseUpdateDto.Address;
            warehouse.Zones = warehouseUpdateDto.Zones ?? warehouse.Zones;
            warehouse.Name = warehouseUpdateDto.Name;
            await _context.SaveChangesAsync();
            return warehouse;
        }
    }
}

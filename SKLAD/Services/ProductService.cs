using Microsoft.EntityFrameworkCore;
using SKLAD.Database;
using SKLAD.Dto;
using SKLAD.Entities;

namespace SKLAD.Services
{
    // объясняю почему я писал логику в контроллере, честно я очень устал все выносить и делать абстракции лишние, а потом их в Program.cs заносить,
    // если открыть его, то там сори за мат пиздец
    public class ProductService(WarehouseDbContext context)
    {
        private readonly WarehouseDbContext _context = context;
        public async Task<List<Product>> GetAsync()
        {
            return await _context.Products.Include(x => x.Movements).ToListAsync();
        }
        public async Task<int> ReceiveGoodsAsync(ReceivingRequest request)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.SKU == request.Barcode);

            if (product == null)
                throw new Exception("Товар не найден");

            // обновление остатков
            product.Quantity += request.Quantity;
            product.ExpiryDate = request.ExpiryDate ?? product.ExpiryDate;
            product.StorageZoneId = request.ZoneId;

            // запись перемещения
            _context.ProductMovements.Add(new ProductMovement
            {
                ProductId = product.Id,
                ToZoneId = request.ZoneId,
                Quantity = request.Quantity,
                MovementDate = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return product.Quantity;
        }
        public async Task<(Product product, StorageZone fromZone, StorageZone toZone)> TransferProductAsync(ProductTransferDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var product = await _context.Products
                    .Include(p => p.StorageZone)
                    .FirstOrDefaultAsync(p => p.Id == dto.ProductId);

                var fromZone = await _context.StorageZones.FindAsync(dto.FromZoneId);
                var toZone = await _context.StorageZones.FindAsync(dto.ToZoneId);

                if (product == null || fromZone == null || toZone == null)
                    throw new Exception("Объект не найден");

                if (toZone.Type != product.StorageRequirements)
                    throw new Exception("Несовместимый тип зоны");

                if (toZone.Capacity - toZone.CurrentStock < dto.Quantity)
                    throw new Exception("Недостаточно места в целевой зоне");

                if (product.Quantity < dto.Quantity)
                    throw new Exception("Недостаточное количество товара");

                // меняем количество
                product.Quantity -= dto.Quantity;
                fromZone.CurrentStock -= dto.Quantity;
                toZone.CurrentStock += dto.Quantity;

                product.StorageZoneId = dto.ToZoneId;

                _context.ProductMovements.Add(new ProductMovement
                {
                    ProductId = product.Id,
                    FromZoneId = dto.FromZoneId,
                    ToZoneId = dto.ToZoneId,
                    Quantity = dto.Quantity,
                    MovementDate = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (product, fromZone, toZone);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Ошибка: {ex.Message}");
            }
        }
        public async Task<(Product product, StorageZone fromZome)> ShipProductAsync(ProductShipDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var product = await _context.Products
                    .Include(p => p.StorageZone)
                    .FirstOrDefaultAsync(p => p.Id == dto.ProductId);

                if (product == null)
                    throw new Exception("Товар не найден");

                if (product.Quantity < dto.Quantity)
                    throw new Exception("Недостаточное количество товара");

                product.Quantity -= dto.Quantity;

                var fromZone = await _context.StorageZones.FindAsync(dto.FromZoneId);
                fromZone.CurrentStock -= dto.Quantity;


                _context.ProductMovements.Add(new ProductMovement
                {
                    ProductId = product.Id,
                    FromZoneId = dto.FromZoneId,
                    Quantity = -dto.Quantity, // для отгрузки делается отрицательным
                    MovementDate = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return (product, fromZone);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Ошибка: {ex.Message}");
            }
        }
        public async Task<Product> CreateAsync(ProductCreateDto productDto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = productDto.Name,
                SKU = productDto.SKU,
                Weight = productDto.Weight,
                MinStockLevel = productDto.MinStockLevel,
                ExpiryDate = productDto.ExpiryDate,
                StorageRequirements = productDto.StorageRequirements,
                StorageZoneId = productDto.TargetZoneId,
                SystemQuantity = productDto.Quantity,
                Quantity = productDto.Quantity,
                LastUpdated = DateTime.UtcNow
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task<Product> ReceiveProductAsync(ProductReceiveDto dto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                SKU = dto.SKU,
                Name = dto.Name,
                Quantity = dto.Quantity,
                Weight = dto.Weight,
                ExpiryDate = dto.ExpiryDate,
                StorageRequirements = dto.StorageRequirements,
                StorageZoneId = dto.TargetZoneId
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }

    }
}

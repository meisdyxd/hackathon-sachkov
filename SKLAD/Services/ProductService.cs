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

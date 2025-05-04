using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKLAD.Database;
using SKLAD.Dto;
using SKLAD.Entities;
using SKLAD.Services;

namespace SKLAD.Controllers
{
    // в общем тут почти полная работа с продуктом, не полностью реализован круд, потому что не вижу в нем смысла, идея ведь в складе(или надо? хз)
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly PlacementService _placementService;
        private readonly WarehouseDbContext _context;
        public ProductsController(ProductService productService, PlacementService placementService, WarehouseDbContext warehouseDbContext)
        {
            _productService = productService;
            _placementService = placementService;
            _context = warehouseDbContext;
        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _productService.GetAsync());
        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductCreateDto productDto)
        {
            return Ok(await _productService.CreateAsync(productDto));
        }
        [HttpPost("receive")] // приемка товароу
        public async Task<IActionResult> ReceiveProduct(ProductReceiveDto productDto)
        {
            var product = await _productService.ReceiveProductAsync(productDto);
            return Ok(product);
        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost("ship")]
        public async Task<IActionResult> ShipProduct(ProductShipDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(); 

            try
            {
                var product = await _context.Products
                    .Include(p => p.StorageZone)
                    .FirstOrDefaultAsync(p => p.Id == dto.ProductId);

                if (product == null)
                    return NotFound("Товар не найден");

                if (product.Quantity < dto.Quantity)
                    return BadRequest("Недостаточное количество товара");

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

                return Ok(new
                {
                    product.Id,
                    RemainingQuantity = product.Quantity,
                    FromZone = fromZone.Name
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Ошибка: {ex.Message}");
            }
        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost("transfer")] // перемешение между зонами
        public async Task<IActionResult> TransferProduct(ProductTransferDto dto)
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
                    return NotFound("Объект не найден");

                if (toZone.Type != product.StorageRequirements)
                    return BadRequest("Несовместимый тип зоны");

                if (toZone.Capacity - toZone.CurrentStock < dto.Quantity)
                    return BadRequest("Недостаточно места в целевой зоне");

                if (product.Quantity < dto.Quantity)
                    return BadRequest("Недостаточное количество товара");

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

                return Ok(new
                {
                    Product = product.Name,
                    FromZone = fromZone.Name,
                    ToZone = toZone.Name,
                    NewQuantity = product.Quantity
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Ошибка: {ex.Message}");
            }
        }
        [HttpPost("placement-suggestions")] // определяет лучшую зону, ноо, как будто мусорный метод
        public async Task<IActionResult> GetPlacementSuggestion(string productType)
        {
            var zoneId = await _placementService.FindOptimalZone(productType);
            return Ok(new { SuggestedZoneId = zoneId });
        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost("receivings")]
        public async Task<IActionResult> ReceiveGoods([FromBody] ReceivingRequest request) // не работает, пробивка по штрихкоду типа товаров
        {
            // поиск товара по штрих-коду
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.SKU == request.Barcode);

            if (product == null)
                return NotFound("Товар не найден");

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

            return Ok(new
            {
                Message = "Товар принят",
                CurrentQuantity = product.Quantity
            });
        }
    }
}

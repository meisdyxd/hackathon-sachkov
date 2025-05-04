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
        public ProductsController(ProductService productService, PlacementService placementService)
        {
            _productService = productService;
            _placementService = placementService;
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
            (var product, var fromZone) = await _productService.ShipProductAsync(dto);
            return Ok(new { product.Id, RemainingQuantity = product.Quantity, FromZone = fromZone.Name });
        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost("transfer")] // перемешение между зонами
        public async Task<IActionResult> TransferProduct(ProductTransferDto dto)
        {
            (var product, var fromZone, var toZone) = await _productService.TransferProductAsync(dto);
                return Ok(new{ Product = product.Name, FromZone = fromZone.Name, ToZone = toZone.Name, NewQuantity = product.Quantity});
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
            return Ok(new { Message = "Товар принят", CurrentQuantity = await _productService.ReceiveGoodsAsync(request) });
        }
    }
}

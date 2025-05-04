using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SKLAD.Dto;
using SKLAD.Services;

namespace SKLAD.Controllers
{
    // круд по работе со складом, ничего интересного
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        public WarehouseController(WarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }
        private readonly WarehouseService _warehouseService;
        [HttpGet]
        public async Task<IActionResult> GetAllWarehouses()
        {
            return Ok(await _warehouseService.GetAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarehouse(WarehouseCreateDto warehouseDto)
        {
            return Ok(await _warehouseService.CreateAsync(warehouseDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWarehouse(Guid id, WarehouseUpdateDto warehouse)
        {
            return Ok(await _warehouseService.UpdateAsync(id, warehouse));
        }
    }
}

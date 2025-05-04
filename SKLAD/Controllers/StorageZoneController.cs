using Microsoft.AspNetCore.Mvc;
using SKLAD.Dto;
using SKLAD.Services;

namespace SKLAD.Controllers
{
    // круд зоны хранений, с методом на проверку зоны на вместительность
    [ApiController]
    [Route("api/zones")]
    public class StorageZoneController : ControllerBase
    {
        public StorageZoneController(StorageZoneService storageZoneService)
        {
            _storageZoneService = storageZoneService;
        }
        private readonly StorageZoneService _storageZoneService;
        [HttpGet("{warehouseId}")]
        public async Task<IActionResult> GetZonesByWarehouse(Guid warehouseId)
        {
            return Ok(await _storageZoneService.GetAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateZone(StorageZoneCreateDto storageZoneDto)
        {
            return Ok(await _storageZoneService.CreateAsync(storageZoneDto));
        }

        [HttpPost("{zoneId}/check-capacity")]
        public async Task<IActionResult> CheckCapacity(Guid zoneId, int requiredSpace)
        {
            return Ok(await _storageZoneService.CheckCapacity(zoneId, requiredSpace));
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKLAD.Database;
using SKLAD.Dto;
using SKLAD.Entities;
using System.Security.Claims;

namespace SKLAD.Controllers
{
    // в общем контроллер для комплектации товаров, я не знаю как правильно работать с координатами, ну чета наговнокодил, генерирует типо маршрут, но у меня он не работает
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly WarehouseDbContext _context;
        public OrderController(WarehouseDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost("picking-tasks")]
        public async Task<IActionResult> CreatePickingTask([FromBody] PickingTask request, double currentPositionX, double currentPositionY)
        {
            var task = new PickingTaskEntity
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderId,
                Items = request.Items,
                AssignedTo = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Status = "New"
            };

            _context.PickingTasks.Add(task);
            await _context.SaveChangesAsync();

            // тут маршрут получаю
            var route = await GeneratePickingRoute(task, currentPositionX, currentPositionY);

            return Ok(new
            {
                TaskId = task.Id,
                Route = route
            });
        }
        private async Task<List<string>> GeneratePickingRoute(PickingTaskEntity task, double currentPositionX, double currentPositionY)
        {
            var route = new List<string>();

            var productIds = task.Items.Select(i => i.ProductId).ToList();

            var zones = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .Select(p => new {
                    p.Id,
                    p.StorageZone.Name,
                    p.StorageZone.Rack,
                    p.StorageZone.Shelf,
                    p.StorageZone.XCoordinate,
                    p.StorageZone.YCoordinate
                })
                .ToListAsync();

            var optimizedRoute = zones
                .OrderBy(z => Math.Sqrt(
                    Math.Pow(z.XCoordinate - currentPositionX, 2) +
                    Math.Pow(z.YCoordinate - currentPositionY, 2)))
                .ThenBy(z => z.Rack)
                .ThenBy(z => z.Shelf)
                .ToList();

            foreach (var zone in optimizedRoute)
            {
                route.Add(
                    $"{zone.Name} -> Стеллаж {zone.Rack}, Полка {zone.Shelf} " +
                    $"[Координаты: ({zone.XCoordinate}, {zone.YCoordinate})]");
            }

            return route;
        }
    }
}

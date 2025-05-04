using Microsoft.EntityFrameworkCore;
using SKLAD.Database;
using SKLAD.Dto;
using SKLAD.Entities;
using System.Security.Claims;

namespace SKLAD.Services
{
    public class OrderService
    {
        private readonly WarehouseDbContext _context;
        public OrderService(WarehouseDbContext context)
        {
            _context = context;
        }
        public async Task<(PickingTaskEntity task, List<string>)> CreatePickingTask(PickingTask request, double currentPositionX, double currentPositionY, ClaimsPrincipal user)
        {
            var task = new PickingTaskEntity
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderId,
                Items = request.Items,
                AssignedTo = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Status = "New"
            };

            _context.PickingTasks.Add(task);
            await _context.SaveChangesAsync();

            // тут маршрут получаю
            var route = await GeneratePickingRoute(task, currentPositionX, currentPositionY);
            return (task, route);
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

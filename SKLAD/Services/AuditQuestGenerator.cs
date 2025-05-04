using Microsoft.EntityFrameworkCore;
using SKLAD.Database;
using SKLAD.Entities;

namespace SKLAD.Services
{
    // те самые квестики, ничего сложного вроде, ноооооооо, чтобы все работало хорошо, должны существовать зоны и товары иначе )))
    public class AuditQuestGenerator
    {
        private readonly WarehouseDbContext _context;
        private readonly Random _random = new();

        public AuditQuestGenerator(WarehouseDbContext context)
        {
            _context = context;
        }

        public Task<AuditQuest> GenerateRandomQuest()
        {
            var questTypes = new[]
                {
                new Func<Task<AuditQuest>>(GenerateExpiredCheckQuest),
                new Func<Task<AuditQuest>>(GenerateZoneCheckQuest)
            };

            return questTypes[_random.Next(questTypes.Length)]();
        }

        private async Task<AuditQuest> GenerateExpiredCheckQuest()
        {
            var count = await _context.Products.CountAsync(p => p.ExpiryDate < DateTime.UtcNow);
            return new AuditQuest
            {
                Title = $"Найди {count} просроченных товаров!",
                Description = "Проверь все зоны склада на наличие просрочки",
                CreatedAt = DateTime.UtcNow
            };
        }

        private async Task<AuditQuest> GenerateZoneCheckQuest()
        {
            var zone = await GetRandomZoneAsync();

            return new AuditQuest
            {
                Title = $"Проверь зону {zone.Name}!",
                Description = "Убедись, что все товары на своих местах",
                CreatedAt = DateTime.UtcNow
            };
        }
        private async Task<StorageZone> GetRandomZoneAsync()
        {
            var zones = await _context.StorageZones
                .ToListAsync();

            if (!zones.Any())
                return null;

            var randomZone = zones
                .OrderBy(_ => _random.Next())
                .First();

            return randomZone;
        }
    }
}

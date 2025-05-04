using Microsoft.EntityFrameworkCore;
using SKLAD.Database;
using SKLAD.Entities;


namespace SKLAD.Services
{
    // лишний слой абстракций, лучше бы не добавлял его ваще. тут квесты
    public class QuestService
    {
        private readonly AuditQuestGenerator _auditQuestGenerator;
        private readonly WarehouseDbContext _context;

        public QuestService(AuditQuestGenerator auditQuestGenerator, WarehouseDbContext context)
        {
            _auditQuestGenerator = auditQuestGenerator;
            _context = context;
        }

        public async Task<AuditQuest> GenerateNewQuest()
        {
            var quest = await _auditQuestGenerator.GenerateRandomQuest();
            await _context.AuditQuests.AddAsync(quest);
            await _context.SaveChangesAsync();
            return quest;
        }

        public async Task<AuditQuest> CompleteQuest(Guid id)
        {
            var quest = await _context.AuditQuests.FindAsync(id);
            quest.IsCompleted = true;
            quest.CompletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return quest;
        }

        public async Task<List<AuditQuest>> GetAsync()
        {
            var quests = await _context.AuditQuests.ToListAsync();
            return quests;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SKLAD.Services;

namespace SKLAD.Controllers
{
    // квесты для работников, Я ХОТЕЛ ЗАСУНУТЬ В БЕКГРАУНД СЕРВИСЫ, НО ЭТО СЛОЖНО. там какая логика то, еще ж нужно отслеживать кому придет квест(а влдруг работник не на работе)
    // тогда бы пришлось какой нибудь проходной пункт добавлять, который там карточкой помечает на работе сотрудник или нет
    // ну это в общем идея интересная как по мне, можно другие квесты добавить, но из-за лоу лвл скилла, не смогу реализовать
    [ApiController]
    [Route("api/[controller]")]
    public class QuestController : ControllerBase
    {
        private readonly QuestService _questService;

        public QuestController(QuestService questService)
        {
            _questService = questService;
        }

        [HttpPost("generate")] 
        public async Task<IActionResult> GenerateNewQuest()
        {
            var quest = await _questService.GenerateNewQuest();
            return Ok(quest);
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> CompleteQuest(Guid id)
        {
            var quest = await _questService.CompleteQuest(id);
            return Ok(quest);
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _questService.GetAsync());
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SKLAD.Services;

namespace SKLAD.Controllers
{
    // в общем, я хотел добавить сюда нейросетку, но идея как появилась так и умерла, я ни разу не работал с нейросетками.
    // идея была в том, что она должна была предсказывать, какие товары закончатся в последуюище 3 дня, но как это все сделать я не знаю
    // время на изучения нейросетей у меня нет(хакатон 2 дня)
    // поставил какой то псведо ии)))
    // он по логике должен работать
    [ApiController]
    [Route("api/[controller]")]
    public class ShortageController : ControllerBase
    {
        private readonly ShortagePredictor _predictor;

        public ShortageController(ShortagePredictor predictor)
        {
            _predictor = predictor;
        }

        [HttpGet]
        public async Task<IActionResult> GetPredictedShortages()
        {
            var shortages = await _predictor.PredictShortages();
            return Ok(shortages);
        }
    }
}

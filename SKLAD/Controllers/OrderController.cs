using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKLAD.Database;
using SKLAD.Dto;
using SKLAD.Entities;
using SKLAD.Services;
using System.Security.Claims;

namespace SKLAD.Controllers
{
    // в общем контроллер для комплектации товаров, я не знаю как правильно работать с координатами, ну чета наговнокодил, генерирует типо маршрут, но у меня он не работает
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost("picking-tasks")]
        public async Task<IActionResult> CreatePickingTask([FromBody] PickingTask request, double currentPositionX, double currentPositionY)
        {
            (var task, var route) = await _orderService.CreatePickingTask(request, currentPositionX, currentPositionY, User);
            return Ok(new
            {
                TaskId = task.Id,
                Route = route
            });
        }
        
    }
}

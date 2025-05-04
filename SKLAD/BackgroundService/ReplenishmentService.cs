//namespace SKLAD.BackgroundServices
//{
//    [HostedService]
//    public class ReplenishmentService : BackgroundService
//    {
//        protected override async Task ExecuteAsync(CancellationToken ct)
//        {
//            while (!ct.IsCancellationRequested)
//            {
//                var lowStockProducts = await _context.Products
//                    .Where(p => p.Quantity < p.MinStockLevel)
//                    .ToListAsync();

//                foreach (var product in lowStockProducts)
//                {
//                    await _purchaseService.CreateOrder(product);
//                }
//                await Task.Delay(TimeSpan.FromHours(1), ct);
//            }
//        }
//    }
//}
//                    ЗДЕСЬ Я ХОТЕЛ ЧТОБЫ ТИПО АВТОМАТОМ ЗАКАЗЫВАЛИСЬ ТОВАРЫ, КОГДА МИНИМАЛЬНЫЙ СТОК БЫЛ БОЛЬШЕ КОЛ-ВА, НО ЧЕТ ПОШЛО НЕ ПО ПЛАНУ, СЛИШКОМ СЛОЖНО
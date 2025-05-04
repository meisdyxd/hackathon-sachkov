using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using SKLAD.Database;
using SKLAD.Dto;
using SKLAD.Entities;

namespace SKLAD.Services
{
    // это тот самый ИИ???? если посмотреть в using я хотел реально подкрутить машинлернинг, но посчитал не круто юзать ИИ для создания ИИ на хакатоне.
    public class ShortagePredictor
    {
        private readonly WarehouseDbContext _context;

        public ShortagePredictor(WarehouseDbContext context)
        {
            _context = context;
        }

        public async Task<List<PredictedShortageDto>> PredictShortages()
        {
            var products = await _context.Products
                .Include(p => p.Movements) 
                .ToListAsync();

            var predictions = new List<Product>();

            foreach (var product in products)
            {
                if (product.Movements.Count == 0) continue;

                var avgDailySales = product.Movements
                    .Average(m => m.Quantity);

                var daysUntilShortage = (product.Quantity - product.MinStockLevel) / avgDailySales;

                if (daysUntilShortage <= 3)
                {
                    predictions.Add(product);
                }
            }

            var selectedPredictions = predictions
                .Select(p => new PredictedShortageDto(p.Id, p.Name, (p.Quantity - p.MinStockLevel) / (p.Movements.Average(m => m.Quantity))))
                .ToList();

            return selectedPredictions;
        }
    }
}

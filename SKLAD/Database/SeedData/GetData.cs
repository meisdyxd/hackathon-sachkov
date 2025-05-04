//using SKLAD.Entities;

//namespace SKLAD.Database.SeedData
//{
//    public static class GetData
//    {
//        public static List<Product> GetSeedProducts()
//        {
//            return new List<Product>
//            {
//                new Product
//                {
//                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
//                    Name = "Молоко 'Домик в деревне'",
//                    Quantity = 150,
//                    MinStockLevel = 50,
//                    ExpiryDate = new DateTime(2025, 05, 10, 12, 0, 0, DateTimeKind.Utc),
//                    StorageZone = "A1"
//                },
//                new Product
//                {
//                    Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
//                    Name = "Крупа гречневая",
//                    Quantity = 80,
//                    MinStockLevel = 30,
//                    ExpiryDate = new DateTime(2025, 11, 3, 12, 0, 0, DateTimeKind.Utc),
//                    StorageZone = "B2"
//                },
//                new Product
//                {
//                    Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
//                    Name = "Ноутбук Lenovo",
//                    Quantity = 15,
//                    MinStockLevel = 5,
//                    ExpiryDate = new DateTime(2027, 5, 3, 12, 0, 0, DateTimeKind.Utc),
//                    StorageZone = "C3"
//                }
//            };
//        }
//        private static List<ProductMovement> GetSeedMovements()
//        {
//            return new List<ProductMovement>
//            {
//                new ProductMovement
//                {
//                    Id = Guid.Parse("00000000-0000-0000-0000-000000000101"),
//                    ProductId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
//                    QuantityMoved = -20, // Минус = продажа/отгрузка
//                    MovementDate = DateTime.Now.AddDays(-3)
//                },
//                new ProductMovement
//                {
//                    Id = Guid.Parse("00000000-0000-0000-0000-000000000102"),
//                    ProductId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
//                    QuantityMoved = -30,
//                    MovementDate = DateTime.Now.AddDays(-1)
//                },
//                new ProductMovement
//                {
//                    Id = Guid.Parse("00000000-0000-0000-0000-000000000201"),
//                    ProductId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
//                    QuantityMoved = -10,
//                    MovementDate = DateTime.Now.AddDays(-2)
//                }
//            };
//        }
//    }
//}

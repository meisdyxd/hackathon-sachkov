using System.ComponentModel.DataAnnotations.Schema;

namespace SKLAD.Entities
{
    [Table("products")]
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public decimal Weight { get; set; }
        public int MinStockLevel { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string StorageRequirements { get; set; }
        public Guid? StorageZoneId { get; set; }
        public StorageZone? StorageZone { get; set; }

        [Column("system_quantity")]
        public int SystemQuantity { get; set; } // Количество по данным системы

        [Column("physical_quantity")]
        public int Quantity { get; set; }
        [Column("last_updated")]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public List<ProductMovement> Movements { get; set; } = new();
    }
}

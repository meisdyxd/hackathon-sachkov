namespace SKLAD.Entities
{
    public class ProductMovement
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid? FromZoneId { get; set; }
        public StorageZone FromZone { get; set; }
        public Guid? ToZoneId { get; set; }
        public StorageZone ToZone { get; set; }
        public int Quantity { get; set; }
        public DateTime MovementDate { get; set; }
    }
}

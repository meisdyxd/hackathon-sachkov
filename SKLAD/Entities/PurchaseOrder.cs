namespace SKLAD.Entities
{
    public class PurchaseOrder
    {
        public Guid Id { get; set; }
        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> Items { get; set; } = new();
    }
}

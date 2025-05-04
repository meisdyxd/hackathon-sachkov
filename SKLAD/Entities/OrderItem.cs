namespace SKLAD.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public Guid ProductId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public int Quantity { get; set; }
    }
}

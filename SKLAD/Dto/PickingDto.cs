using SKLAD.Entities;

namespace SKLAD.Dto
{
    public record PickingTask(Guid OrderId, List<PickingItem> Items);

    public class PickingItem
    {
        public Guid Id { get; set; }
        public Guid PickingTaskId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public PickingTaskEntity PickingTask { get; set; }
        public Product Product { get; set; }
        public PickingItem() { }
        public PickingItem(Guid ProductId, int Quantity) { }
    }


}

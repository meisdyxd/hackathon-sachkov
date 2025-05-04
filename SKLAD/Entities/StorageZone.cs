namespace SKLAD.Entities
{
    public class StorageZone
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Capacity { get; set; }
        public int CurrentStock { get; set; }
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public int Rack { get; set; }
        public int Shelf { get; set; }
        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}

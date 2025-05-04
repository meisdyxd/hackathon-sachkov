namespace SKLAD.Entities
{
    public class Warehouse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<StorageZone>? Zones { get; set; } = new();
    }
}

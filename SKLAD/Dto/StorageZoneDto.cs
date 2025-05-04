namespace SKLAD.Dto
{
    public record StorageZoneCreateDto(string Name, string Type, int Capacity, int CurrentStock, int XCoordinate, int YCoordinate, int Rack, int Shelf, Guid WarehouseId);
    public record StorageZoneResponseDto(Guid Id, string Name, string Type, int Capacity, int CurrentStock, int XCoordinate, int YCoordinate, int Rack, int Shelf, Guid WarehouseId);
}

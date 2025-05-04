namespace SKLAD.Dto
{
    public record ProductReceiveDto(string SKU, string Name, int Quantity, decimal Weight, DateTime ExpiryDate, string StorageRequirements, Guid TargetZoneId);
    public record ProductShipDto(Guid ProductId, int Quantity, Guid FromZoneId);
    public record ProductTransferDto(Guid ProductId, int Quantity, Guid FromZoneId, Guid ToZoneId);
    public record ProductCreateDto(string SKU, string Name, int Quantity, decimal Weight, int MinStockLevel, DateTime ExpiryDate, string StorageRequirements, Guid TargetZoneId);
}

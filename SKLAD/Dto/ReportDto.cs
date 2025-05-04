namespace SKLAD.Dto
{
    public record StockReportDto(Guid ProductId, string ProductName, string SKU, string ZoneName, int CurrentQuantity, int AvailableSpace);
    public record ExpirationReportDto(Guid ProductId, string ProductName, string SKU, DateTime ExpiryDate, int DaysUntilExpiration);
}

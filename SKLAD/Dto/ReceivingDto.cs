namespace SKLAD.Dto
{
    public record ReceivingRequest(string Barcode, int Quantity, Guid ZoneId, DateTime? ExpiryDate);
}

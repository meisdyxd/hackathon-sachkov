namespace SKLAD.Dto
{
    public record PredictedShortageDto(Guid Id, string Name, double DaysUntilShortage);
}

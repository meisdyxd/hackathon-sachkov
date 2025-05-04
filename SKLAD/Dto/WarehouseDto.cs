using SKLAD.Entities;

namespace SKLAD.Dto
{
    public record WarehouseUpdateDto(string Name, string Address, List<StorageZone>? Zones);
    public record WarehouseCreateDto(string Name, string Address);
    public record WarehouseResponse(Guid Id, string Name, string Address, List<StorageZoneResponseDto> Zones);
}

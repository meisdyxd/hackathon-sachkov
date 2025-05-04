using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SKLAD.Migrations
{
    /// <inheritdoc />
    public partial class coordinatesandways : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rack",
                table: "StorageZones",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Shelf",
                table: "StorageZones",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "XCoordinate",
                table: "StorageZones",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YCoordinate",
                table: "StorageZones",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rack",
                table: "StorageZones");

            migrationBuilder.DropColumn(
                name: "Shelf",
                table: "StorageZones");

            migrationBuilder.DropColumn(
                name: "XCoordinate",
                table: "StorageZones");

            migrationBuilder.DropColumn(
                name: "YCoordinate",
                table: "StorageZones");
        }
    }
}

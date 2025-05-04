using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SKLAD.Migrations
{
    /// <inheritdoc />
    public partial class seeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "ExpiryDate", "MinStockLevel", "Name", "Quantity", "StorageZone" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 5, 10, 12, 0, 0, 0, DateTimeKind.Utc), 50, "Молоко 'Домик в деревне'", 150, "A1" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2025, 11, 3, 12, 0, 0, 0, DateTimeKind.Utc), 30, "Крупа гречневая", 80, "B2" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2027, 5, 3, 12, 0, 0, 0, DateTimeKind.Utc), 5, "Ноутбук Lenovo", 15, "C3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));
        }
    }
}

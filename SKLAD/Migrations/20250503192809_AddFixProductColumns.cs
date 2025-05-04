using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SKLAD.Migrations
{
    /// <inheritdoc />
    public partial class AddFixProductColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMovements_Products_ProductId",
                table: "ProductMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMovements_Products_ProductId1",
                table: "ProductMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_StorageZones_StorageZoneId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "products");

            migrationBuilder.RenameIndex(
                name: "IX_Products_StorageZoneId",
                table: "products",
                newName: "IX_products_StorageZoneId");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_updated",
                table: "products",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_products",
                table: "products",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMovements_products_ProductId",
                table: "ProductMovements",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMovements_products_ProductId1",
                table: "ProductMovements",
                column: "ProductId1",
                principalTable: "products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_products_StorageZones_StorageZoneId",
                table: "products",
                column: "StorageZoneId",
                principalTable: "StorageZones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMovements_products_ProductId",
                table: "ProductMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMovements_products_ProductId1",
                table: "ProductMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_products_StorageZones_StorageZoneId",
                table: "products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_products",
                table: "products");

            migrationBuilder.DropColumn(
                name: "last_updated",
                table: "products");

            migrationBuilder.RenameTable(
                name: "products",
                newName: "Products");

            migrationBuilder.RenameIndex(
                name: "IX_products_StorageZoneId",
                table: "Products",
                newName: "IX_Products_StorageZoneId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMovements_Products_ProductId",
                table: "ProductMovements",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMovements_Products_ProductId1",
                table: "ProductMovements",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_StorageZones_StorageZoneId",
                table: "Products",
                column: "StorageZoneId",
                principalTable: "StorageZones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

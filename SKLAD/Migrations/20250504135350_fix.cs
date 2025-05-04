using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using SKLAD.Dto;

#nullable disable

namespace SKLAD.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_StorageZones_StorageZoneId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Items",
                table: "PickingTasks");

            migrationBuilder.AlterColumn<Guid>(
                name: "StorageZoneId",
                table: "products",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "PickingItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PickingTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickingItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PickingItem_PickingTasks_PickingTaskId",
                        column: x => x.PickingTaskId,
                        principalTable: "PickingTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PickingItem_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PickingItem_PickingTaskId",
                table: "PickingItem",
                column: "PickingTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_PickingItem_ProductId",
                table: "PickingItem",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_products_StorageZones_StorageZoneId",
                table: "products",
                column: "StorageZoneId",
                principalTable: "StorageZones",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_StorageZones_StorageZoneId",
                table: "products");

            migrationBuilder.DropTable(
                name: "PickingItem");

            migrationBuilder.AlterColumn<Guid>(
                name: "StorageZoneId",
                table: "products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<List<PickingItem>>(
                name: "Items",
                table: "PickingTasks",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_products_StorageZones_StorageZoneId",
                table: "products",
                column: "StorageZoneId",
                principalTable: "StorageZones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

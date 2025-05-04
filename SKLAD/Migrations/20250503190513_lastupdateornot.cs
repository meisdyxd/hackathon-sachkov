using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using SKLAD.Dto;

#nullable disable

namespace SKLAD.Migrations
{
    /// <inheritdoc />
    public partial class lastupdateornot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Products",
                newName: "physical_quantity");

            migrationBuilder.AddColumn<int>(
                name: "system_quantity",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PickingTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedTo = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Items = table.Column<List<PickingItem>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickingTasks", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PickingTasks");

            migrationBuilder.DropColumn(
                name: "system_quantity",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "physical_quantity",
                table: "Products",
                newName: "Quantity");
        }
    }
}

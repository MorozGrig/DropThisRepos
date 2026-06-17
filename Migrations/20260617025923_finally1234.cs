using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DropThisSite.Migrations
{
    /// <inheritdoc />
    public partial class finally1234 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    IdWarehouse = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameWarehouse = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.IdWarehouse);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseItems",
                columns: table => new
                {
                    IdWarehouseItem = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdWarehouse = table.Column<int>(type: "int", nullable: false),
                    IdJewelry = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseItems", x => x.IdWarehouseItem);
                    table.ForeignKey(
                        name: "FK_WarehouseItems_Jewelries_IdJewelry",
                        column: x => x.IdJewelry,
                        principalTable: "Jewelries",
                        principalColumn: "IdJewelry",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseItems_Warehouses_IdWarehouse",
                        column: x => x.IdWarehouse,
                        principalTable: "Warehouses",
                        principalColumn: "IdWarehouse",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItems_IdJewelry",
                table: "WarehouseItems",
                column: "IdJewelry");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItems_IdWarehouse",
                table: "WarehouseItems",
                column: "IdWarehouse");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WarehouseItems");

            migrationBuilder.DropTable(
                name: "Warehouses");
        }
    }
}

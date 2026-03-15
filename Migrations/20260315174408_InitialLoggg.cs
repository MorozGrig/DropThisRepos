using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DropThisSite.Migrations
{
    /// <inheritdoc />
    public partial class InitialLoggg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jewelries_JewelryTips_IdJewelryType",
                table: "Jewelries");

            migrationBuilder.DropIndex(
                name: "IX_Jewelries_IdJewelryType",
                table: "Jewelries");

            migrationBuilder.DropColumn(
                name: "IdJewelryType",
                table: "Jewelries");

            migrationBuilder.CreateIndex(
                name: "IX_Jewelries_IdJewelryTip",
                table: "Jewelries",
                column: "IdJewelryTip");

            migrationBuilder.AddForeignKey(
                name: "FK_Jewelries_JewelryTips_IdJewelryTip",
                table: "Jewelries",
                column: "IdJewelryTip",
                principalTable: "JewelryTips",
                principalColumn: "IdJewelryTip",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jewelries_JewelryTips_IdJewelryTip",
                table: "Jewelries");

            migrationBuilder.DropIndex(
                name: "IX_Jewelries_IdJewelryTip",
                table: "Jewelries");

            migrationBuilder.AddColumn<int>(
                name: "IdJewelryType",
                table: "Jewelries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jewelries_IdJewelryType",
                table: "Jewelries",
                column: "IdJewelryType");

            migrationBuilder.AddForeignKey(
                name: "FK_Jewelries_JewelryTips_IdJewelryType",
                table: "Jewelries",
                column: "IdJewelryType",
                principalTable: "JewelryTips",
                principalColumn: "IdJewelryTip");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DropThisSite.Migrations
{
    /// <inheritdoc />
    public partial class AddJewelryImagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Jewelries",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Jewelries");
        }
    }
}

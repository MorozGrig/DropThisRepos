using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DropThisSite.Migrations
{
    /// <inheritdoc />
    public partial class finally12345 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdSposobOplati",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SposobiOplati",
                columns: table => new
                {
                    IdSposobOplati = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameSposobOplati = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SposobiOplati", x => x.IdSposobOplati);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_IdSposobOplati",
                table: "Orders",
                column: "IdSposobOplati");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_SposobiOplati_IdSposobOplati",
                table: "Orders",
                column: "IdSposobOplati",
                principalTable: "SposobiOplati",
                principalColumn: "IdSposobOplati",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_SposobiOplati_IdSposobOplati",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "SposobiOplati");

            migrationBuilder.DropIndex(
                name: "IX_Orders_IdSposobOplati",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IdSposobOplati",
                table: "Orders");
        }
    }
}

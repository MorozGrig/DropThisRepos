using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DropThisSite.Migrations
{
    /// <inheritdoc />
    public partial class Dip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JewelryTips",
                columns: table => new
                {
                    IdJewelryTip = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameJewelryTip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JewelryTips", x => x.IdJewelryTip);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    IdMaterial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameMaterial = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Proba = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.IdMaterial);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRole = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameRole = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRole);
                });

            migrationBuilder.CreateTable(
                name: "StatusOrders",
                columns: table => new
                {
                    IdStatusOrder = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameStatusOrder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusOrders", x => x.IdStatusOrder);
                });

            migrationBuilder.CreateTable(
                name: "Stones",
                columns: table => new
                {
                    IdStone = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameStone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ColorStone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WeightStone = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stones", x => x.IdStone);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    IdSupplier = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameSupplier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneSupplier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmailSupplier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.IdSupplier);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdRole = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IdUser);
                    table.ForeignKey(
                        name: "FK_Users_Roles_IdRole",
                        column: x => x.IdRole,
                        principalTable: "Roles",
                        principalColumn: "IdRole",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jewelries",
                columns: table => new
                {
                    IdJewelry = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameJewelry = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdJewelryTip = table.Column<int>(type: "int", nullable: false),
                    IdJewelryType = table.Column<int>(type: "int", nullable: true),
                    IdMaterial = table.Column<int>(type: "int", nullable: false),
                    IdStone = table.Column<int>(type: "int", nullable: false),
                    IdSupplier = table.Column<int>(type: "int", nullable: false),
                    PriceJewelry = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jewelries", x => x.IdJewelry);
                    table.ForeignKey(
                        name: "FK_Jewelries_JewelryTips_IdJewelryType",
                        column: x => x.IdJewelryType,
                        principalTable: "JewelryTips",
                        principalColumn: "IdJewelryTip");
                    table.ForeignKey(
                        name: "FK_Jewelries_Materials_IdMaterial",
                        column: x => x.IdMaterial,
                        principalTable: "Materials",
                        principalColumn: "IdMaterial",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Jewelries_Stones_IdStone",
                        column: x => x.IdStone,
                        principalTable: "Stones",
                        principalColumn: "IdStone",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Jewelries_Suppliers_IdSupplier",
                        column: x => x.IdSupplier,
                        principalTable: "Suppliers",
                        principalColumn: "IdSupplier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    IdOrder = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    IdJewelry = table.Column<int>(type: "int", nullable: false),
                    IdStatusOrder = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.IdOrder);
                    table.ForeignKey(
                        name: "FK_Orders_Jewelries_IdJewelry",
                        column: x => x.IdJewelry,
                        principalTable: "Jewelries",
                        principalColumn: "IdJewelry",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_StatusOrders_IdStatusOrder",
                        column: x => x.IdStatusOrder,
                        principalTable: "StatusOrders",
                        principalColumn: "IdStatusOrder",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jewelries_IdJewelryType",
                table: "Jewelries",
                column: "IdJewelryType");

            migrationBuilder.CreateIndex(
                name: "IX_Jewelries_IdMaterial",
                table: "Jewelries",
                column: "IdMaterial");

            migrationBuilder.CreateIndex(
                name: "IX_Jewelries_IdStone",
                table: "Jewelries",
                column: "IdStone");

            migrationBuilder.CreateIndex(
                name: "IX_Jewelries_IdSupplier",
                table: "Jewelries",
                column: "IdSupplier");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_IdJewelry",
                table: "Orders",
                column: "IdJewelry");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_IdStatusOrder",
                table: "Orders",
                column: "IdStatusOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_IdUser",
                table: "Orders",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdRole",
                table: "Users",
                column: "IdRole");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Jewelries");

            migrationBuilder.DropTable(
                name: "StatusOrders");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "JewelryTips");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Stones");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}

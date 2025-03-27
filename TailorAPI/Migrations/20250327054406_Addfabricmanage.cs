using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TailorAPI.Migrations
{
    /// <inheritdoc />
    public partial class Addfabricmanage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FabricTypeID",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FabricTypes",
                columns: table => new
                {
                    FabricTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FabricName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PricePerMeter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvailableStock = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FabricTypes", x => x.FabricTypeID);
                });

            migrationBuilder.CreateTable(
                name: "FabricStocks",
                columns: table => new
                {
                    StockID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FabricTypeID = table.Column<int>(type: "int", nullable: false),
                    StockIn = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockUse = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockAddDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FabricStocks", x => x.StockID);
                    table.ForeignKey(
                        name: "FK_FabricStocks_FabricTypes_FabricTypeID",
                        column: x => x.FabricTypeID,
                        principalTable: "FabricTypes",
                        principalColumn: "FabricTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_FabricTypeID",
                table: "Orders",
                column: "FabricTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_FabricStocks_FabricTypeID",
                table: "FabricStocks",
                column: "FabricTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_FabricTypes_FabricTypeID",
                table: "Orders",
                column: "FabricTypeID",
                principalTable: "FabricTypes",
                principalColumn: "FabricTypeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_FabricTypes_FabricTypeID",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "FabricStocks");

            migrationBuilder.DropTable(
                name: "FabricTypes");

            migrationBuilder.DropIndex(
                name: "IX_Orders_FabricTypeID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FabricTypeID",
                table: "Orders");
        }
    }
}

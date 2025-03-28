using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TailorAPI.Migrations
{
    /// <inheritdoc />
    public partial class Remove_TableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Fabrics_FabricID",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Fabrics");

            migrationBuilder.DropIndex(
                name: "IX_Orders_FabricID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FabricID",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FabricID",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Fabrics",
                columns: table => new
                {
                    FabricID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FabricName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FabricUsed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PricePerMeter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fabrics", x => x.FabricID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_FabricID",
                table: "Orders",
                column: "FabricID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Fabrics_FabricID",
                table: "Orders",
                column: "FabricID",
                principalTable: "Fabrics",
                principalColumn: "FabricID");
        }
    }
}

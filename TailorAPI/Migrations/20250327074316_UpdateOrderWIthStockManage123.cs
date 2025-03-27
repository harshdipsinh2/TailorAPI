using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TailorAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderWIthStockManage123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Fabrics_FabricID",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "FabricID",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Fabrics_FabricID",
                table: "Orders",
                column: "FabricID",
                principalTable: "Fabrics",
                principalColumn: "FabricID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Fabrics_FabricID",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "FabricID",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Fabrics_FabricID",
                table: "Orders",
                column: "FabricID",
                principalTable: "Fabrics",
                principalColumn: "FabricID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

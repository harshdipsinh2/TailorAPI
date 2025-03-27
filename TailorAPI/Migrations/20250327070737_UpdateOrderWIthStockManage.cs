using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TailorAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderWIthStockManage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_FabricTypes_FabricTypeID",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "FabricTypeID",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_FabricTypes_FabricTypeID",
                table: "Orders",
                column: "FabricTypeID",
                principalTable: "FabricTypes",
                principalColumn: "FabricTypeID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_FabricTypes_FabricTypeID",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "FabricTypeID",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_FabricTypes_FabricTypeID",
                table: "Orders",
                column: "FabricTypeID",
                principalTable: "FabricTypes",
                principalColumn: "FabricTypeID");
        }
    }
}

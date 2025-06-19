using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TailorAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddShopAndranchToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Branches_BranchId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Branches_BranchId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Shops_ShopId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "ShopId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId1",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShopId1",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BranchId1",
                table: "Orders",
                column: "BranchId1");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShopId1",
                table: "Orders",
                column: "ShopId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Branches_BranchId",
                table: "Customers",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Branches_BranchId",
                table: "Orders",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Branches_BranchId1",
                table: "Orders",
                column: "BranchId1",
                principalTable: "Branches",
                principalColumn: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Shops_ShopId",
                table: "Orders",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ShopId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Shops_ShopId1",
                table: "Orders",
                column: "ShopId1",
                principalTable: "Shops",
                principalColumn: "ShopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Branches_BranchId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Branches_BranchId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Branches_BranchId1",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Shops_ShopId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Shops_ShopId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BranchId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShopId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BranchId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShopId1",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "ShopId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Branches_BranchId",
                table: "Customers",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Branches_BranchId",
                table: "Orders",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Shops_ShopId",
                table: "Orders",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ShopId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TailorAPI.Migrations
{
    /// <inheritdoc />
    public partial class changes2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Branches_BranchId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Shops_ShopId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "TwilioSms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "TwilioSms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                name: "BranchId",
                table: "Measurements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "Measurements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "FabricTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "FabricTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "FabricStocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "FabricStocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TwilioSms_BranchId",
                table: "TwilioSms",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TwilioSms_ShopId",
                table: "TwilioSms",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BranchId",
                table: "Products",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ShopId",
                table: "Products",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_BranchId",
                table: "Measurements",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_ShopId",
                table: "Measurements",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricTypes_BranchId",
                table: "FabricTypes",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricTypes_ShopId",
                table: "FabricTypes",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricStocks_BranchId",
                table: "FabricStocks",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricStocks_ShopId",
                table: "FabricStocks",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_FabricStocks_Branches_BranchId",
                table: "FabricStocks",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FabricStocks_Shops_ShopId",
                table: "FabricStocks",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ShopId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FabricTypes_Branches_BranchId",
                table: "FabricTypes",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FabricTypes_Shops_ShopId",
                table: "FabricTypes",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ShopId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_Branches_BranchId",
                table: "Measurements",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_Shops_ShopId",
                table: "Measurements",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ShopId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Branches_BranchId",
                table: "Orders",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Shops_ShopId",
                table: "Orders",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ShopId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Branches_BranchId",
                table: "Products",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Shops_ShopId",
                table: "Products",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ShopId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TwilioSms_Branches_BranchId",
                table: "TwilioSms",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TwilioSms_Shops_ShopId",
                table: "TwilioSms",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ShopId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FabricStocks_Branches_BranchId",
                table: "FabricStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_FabricStocks_Shops_ShopId",
                table: "FabricStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_FabricTypes_Branches_BranchId",
                table: "FabricTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_FabricTypes_Shops_ShopId",
                table: "FabricTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_Branches_BranchId",
                table: "Measurements");

            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_Shops_ShopId",
                table: "Measurements");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Branches_BranchId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Shops_ShopId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Branches_BranchId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Shops_ShopId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_TwilioSms_Branches_BranchId",
                table: "TwilioSms");

            migrationBuilder.DropForeignKey(
                name: "FK_TwilioSms_Shops_ShopId",
                table: "TwilioSms");

            migrationBuilder.DropIndex(
                name: "IX_TwilioSms_BranchId",
                table: "TwilioSms");

            migrationBuilder.DropIndex(
                name: "IX_TwilioSms_ShopId",
                table: "TwilioSms");

            migrationBuilder.DropIndex(
                name: "IX_Products_BranchId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ShopId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Measurements_BranchId",
                table: "Measurements");

            migrationBuilder.DropIndex(
                name: "IX_Measurements_ShopId",
                table: "Measurements");

            migrationBuilder.DropIndex(
                name: "IX_FabricTypes_BranchId",
                table: "FabricTypes");

            migrationBuilder.DropIndex(
                name: "IX_FabricTypes_ShopId",
                table: "FabricTypes");

            migrationBuilder.DropIndex(
                name: "IX_FabricStocks_BranchId",
                table: "FabricStocks");

            migrationBuilder.DropIndex(
                name: "IX_FabricStocks_ShopId",
                table: "FabricStocks");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "TwilioSms");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "TwilioSms");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "FabricTypes");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "FabricTypes");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "FabricStocks");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "FabricStocks");

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

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TailorAPI.Migrations
{
    /// <inheritdoc />
    public partial class changes1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Shops",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserName",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shops_CreatedByUserId",
                table: "Shops",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_Users_CreatedByUserId",
                table: "Shops",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shops_Users_CreatedByUserId",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Shops_CreatedByUserId",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "CreatedByUserName",
                table: "Shops");
        }
    }
}

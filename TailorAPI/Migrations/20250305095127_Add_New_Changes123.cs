using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TailorAPI.Migrations
{
    /// <inheritdoc />
    public partial class Add_New_Changes123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedTo",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AssignedTo",
                table: "Orders",
                column: "AssignedTo");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_AssignedTo",
                table: "Orders",
                column: "AssignedTo",
                principalTable: "Users",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_AssignedTo",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AssignedTo",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "Orders");
        }
    }
}

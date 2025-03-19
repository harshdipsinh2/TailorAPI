using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TailorAPI.Migrations
{
    /// <inheritdoc />
    public partial class Hasdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FabricUsed",
                table: "Fabrics",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FabricUsed",
                table: "Fabrics");
        }
    }
}

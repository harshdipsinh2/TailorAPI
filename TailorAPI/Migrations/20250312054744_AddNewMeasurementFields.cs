using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TailorAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNewMeasurementFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Ankle",
                table: "Measurements",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Bicep",
                table: "Measurements",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Calf",
                table: "Measurements",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Forearm",
                table: "Measurements",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Wrist",
                table: "Measurements",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ankle",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "Bicep",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "Calf",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "Forearm",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "Wrist",
                table: "Measurements");
        }
    }
}

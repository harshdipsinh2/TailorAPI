using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TailorAPI.Migrations
{
    /// <inheritdoc />
    public partial class Twiliostringupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SmsType",
                table: "TwilioSms",
                type: "nvarchar(24)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SmsType",
                table: "TwilioSms",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(24)");
        }
    }
}

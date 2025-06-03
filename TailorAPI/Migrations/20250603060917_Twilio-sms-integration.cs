using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TailorAPI.Migrations
{
    /// <inheritdoc />
    public partial class Twiliosmsintegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TwilioSms",
                columns: table => new
                {
                    TwilioSmsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SmsType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwilioSms", x => x.TwilioSmsID);
                    table.ForeignKey(
                        name: "FK_TwilioSms_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TwilioSms_OrderID",
                table: "TwilioSms",
                column: "OrderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwilioSms");
        }
    }
}

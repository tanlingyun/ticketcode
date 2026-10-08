using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketCode.WebHost.Migrations
{
    public partial class Update20220510 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "tExpireTime",
                table: "TcRequestLines",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_TcRequestLines_bConsume_tExpireTime",
                table: "TcRequestLines",
                columns: new[] { "bConsume", "tExpireTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TcRequestLines_bConsume_tExpireTime",
                table: "TcRequestLines");

            migrationBuilder.DropColumn(
                name: "tExpireTime",
                table: "TcRequestLines");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketCode.WebHost.Migrations
{
    public partial class AddConsumeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "iConsumeType",
                table: "TcRequestLines",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "iConsumeType",
                table: "TcRequestLines");
        }
    }
}

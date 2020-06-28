using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketCode.WebHost.Migrations
{
    public partial class UpdateOutNoIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TcRequsets_iAccountId_sOuterNo",
                table: "TcRequsets");

            migrationBuilder.AlterColumn<string>(
                name: "sAppName",
                table: "TcAccounts",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10) CHARACTER SET utf8mb4",
                oldMaxLength: 10);

            migrationBuilder.CreateIndex(
                name: "IX_TcRequsets_iAccountId_iGroupId_sOuterNo",
                table: "TcRequsets",
                columns: new[] { "iAccountId", "iGroupId", "sOuterNo" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TcRequsets_iAccountId_iGroupId_sOuterNo",
                table: "TcRequsets");

            migrationBuilder.AlterColumn<string>(
                name: "sAppName",
                table: "TcAccounts",
                type: "varchar(10) CHARACTER SET utf8mb4",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_TcRequsets_iAccountId_sOuterNo",
                table: "TcRequsets",
                columns: new[] { "iAccountId", "sOuterNo" },
                unique: true);
        }
    }
}

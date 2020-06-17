using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketCode.WebHost.Migrations
{
    public partial class InitDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TcAccounts",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sAppName = table.Column<string>(maxLength: 10, nullable: false),
                    sAppId = table.Column<string>(maxLength: 50, nullable: false),
                    sAppSecret = table.Column<string>(maxLength: 50, nullable: false),
                    tCreateTime = table.Column<DateTime>(nullable: false),
                    bDisable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TcAccounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TcGroups",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    iPrefixCode = table.Column<int>(nullable: false),
                    sName = table.Column<string>(maxLength: 10, nullable: false),
                    iLength = table.Column<int>(nullable: false),
                    iUsedNumber = table.Column<int>(nullable: false),
                    iIncrNumber = table.Column<int>(nullable: false),
                    iMinNumber = table.Column<int>(nullable: false),
                    tCreateTime = table.Column<DateTime>(nullable: false),
                    iCurrAvaNumber = table.Column<int>(nullable: false),
                    tUpdateTime = table.Column<DateTime>(nullable: true),
                    bDisable = table.Column<bool>(nullable: false),
                    bDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TcGroups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TcGroupInAccount",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    iGroupId = table.Column<long>(nullable: false),
                    iAccountId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TcGroupInAccount", x => x.id);
                    table.ForeignKey(
                        name: "FK_TcGroupInAccount_TcAccounts_iAccountId",
                        column: x => x.iAccountId,
                        principalTable: "TcAccounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TcGroupInAccount_TcGroups_iGroupId",
                        column: x => x.iGroupId,
                        principalTable: "TcGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TcRequsets",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sRequestNo = table.Column<string>(nullable: false),
                    sOuterNo = table.Column<string>(maxLength: 50, nullable: false),
                    iNumber = table.Column<int>(nullable: false),
                    tCreateTime = table.Column<DateTime>(nullable: false),
                    tExpireTime = table.Column<DateTime>(nullable: false),
                    iGroupId = table.Column<long>(nullable: false),
                    iAccountId = table.Column<long>(nullable: false),
                    sMemo = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TcRequsets", x => x.id);
                    table.ForeignKey(
                        name: "FK_TcRequsets_TcAccounts_iAccountId",
                        column: x => x.iAccountId,
                        principalTable: "TcAccounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TcRequsets_TcGroups_iGroupId",
                        column: x => x.iGroupId,
                        principalTable: "TcGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TcRequestLines",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    iRequestId = table.Column<long>(nullable: false),
                    iCode = table.Column<int>(nullable: false),
                    iFullCode = table.Column<int>(nullable: false),
                    bConsume = table.Column<bool>(nullable: false),
                    tConsumeTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TcRequestLines", x => x.id);
                    table.ForeignKey(
                        name: "FK_TcRequestLines_TcRequsets_iRequestId",
                        column: x => x.iRequestId,
                        principalTable: "TcRequsets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TcConsume",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    iRequestLineId = table.Column<long>(nullable: false),
                    iFullCode = table.Column<int>(nullable: false),
                    tConsumeTime = table.Column<DateTime>(nullable: false),
                    iGroupId = table.Column<long>(nullable: false),
                    iAccountId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TcConsume", x => x.id);
                    table.ForeignKey(
                        name: "FK_TcConsume_TcAccounts_iAccountId",
                        column: x => x.iAccountId,
                        principalTable: "TcAccounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TcConsume_TcGroups_iGroupId",
                        column: x => x.iGroupId,
                        principalTable: "TcGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TcConsume_TcRequestLines_iRequestLineId",
                        column: x => x.iRequestLineId,
                        principalTable: "TcRequestLines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TcAccounts_sAppId",
                table: "TcAccounts",
                column: "sAppId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TcConsume_iAccountId",
                table: "TcConsume",
                column: "iAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TcConsume_iFullCode",
                table: "TcConsume",
                column: "iFullCode");

            migrationBuilder.CreateIndex(
                name: "IX_TcConsume_iGroupId",
                table: "TcConsume",
                column: "iGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TcConsume_iRequestLineId",
                table: "TcConsume",
                column: "iRequestLineId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TcGroupInAccount_iAccountId",
                table: "TcGroupInAccount",
                column: "iAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TcGroupInAccount_iGroupId",
                table: "TcGroupInAccount",
                column: "iGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TcRequestLines_iFullCode",
                table: "TcRequestLines",
                column: "iFullCode");

            migrationBuilder.CreateIndex(
                name: "IX_TcRequestLines_iRequestId",
                table: "TcRequestLines",
                column: "iRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_TcRequsets_iGroupId",
                table: "TcRequsets",
                column: "iGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TcRequsets_iAccountId_sOuterNo",
                table: "TcRequsets",
                columns: new[] { "iAccountId", "sOuterNo" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TcConsume");

            migrationBuilder.DropTable(
                name: "TcGroupInAccount");

            migrationBuilder.DropTable(
                name: "TcRequestLines");

            migrationBuilder.DropTable(
                name: "TcRequsets");

            migrationBuilder.DropTable(
                name: "TcAccounts");

            migrationBuilder.DropTable(
                name: "TcGroups");
        }
    }
}

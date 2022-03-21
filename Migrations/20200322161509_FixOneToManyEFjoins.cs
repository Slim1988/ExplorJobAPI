using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExplorJobAPI.Migrations
{
    public partial class FixOneToManyEFjoins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserContactInformations_Users_UserEntityGuid",
                table: "UserContactInformations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserContactMethods_Users_UserEntityGuid",
                table: "UserContactMethods");

            migrationBuilder.DropIndex(
                name: "IX_UserContactMethods_UserEntityGuid",
                table: "UserContactMethods");

            migrationBuilder.DropIndex(
                name: "IX_UserContactInformations_UserEntityGuid",
                table: "UserContactInformations");

            migrationBuilder.DropColumn(
                name: "UserEntityGuid",
                table: "UserContactMethods");

            migrationBuilder.DropColumn(
                name: "UserEntityGuid",
                table: "UserContactInformations");

            migrationBuilder.CreateTable(
                name: "UserContactInformationJoins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    UserContactInformationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContactInformationJoins", x => new { x.UserId, x.UserContactInformationId });
                    table.ForeignKey(
                        name: "FK_UserContactInformationJoins_UserContactInformations_UserCon~",
                        column: x => x.UserContactInformationId,
                        principalTable: "UserContactInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserContactInformationJoins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserContactMethodJoins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    UserContactMethodId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContactMethodJoins", x => new { x.UserId, x.UserContactMethodId });
                    table.ForeignKey(
                        name: "FK_UserContactMethodJoins_UserContactMethods_UserContactMethod~",
                        column: x => x.UserContactMethodId,
                        principalTable: "UserContactMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserContactMethodJoins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserContactInformationJoins_UserContactInformationId",
                table: "UserContactInformationJoins",
                column: "UserContactInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContactMethodJoins_UserContactMethodId",
                table: "UserContactMethodJoins",
                column: "UserContactMethodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserContactInformationJoins");

            migrationBuilder.DropTable(
                name: "UserContactMethodJoins");

            migrationBuilder.AddColumn<Guid>(
                name: "UserEntityGuid",
                table: "UserContactMethods",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserEntityGuid",
                table: "UserContactInformations",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserContactMethods_UserEntityGuid",
                table: "UserContactMethods",
                column: "UserEntityGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserContactInformations_UserEntityGuid",
                table: "UserContactInformations",
                column: "UserEntityGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_UserContactInformations_Users_UserEntityGuid",
                table: "UserContactInformations",
                column: "UserEntityGuid",
                principalTable: "Users",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserContactMethods_Users_UserEntityGuid",
                table: "UserContactMethods",
                column: "UserEntityGuid",
                principalTable: "Users",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

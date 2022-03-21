using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExplorJobAPI.Migrations
{
    public partial class JobDomainAndUserRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobDomains_JobUsers_JobUserEntityId",
                table: "JobDomains");

            migrationBuilder.DropIndex(
                name: "IX_JobDomains_JobUserEntityId",
                table: "JobDomains");

            migrationBuilder.DropColumn(
                name: "JobUserEntityId",
                table: "JobDomains");

            migrationBuilder.CreateTable(
                name: "JobUserJobDomainJoins",
                columns: table => new
                {
                    JobUserId = table.Column<Guid>(nullable: false),
                    JobDomainId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobUserJobDomainJoins", x => new { x.JobUserId, x.JobDomainId });
                    table.ForeignKey(
                        name: "FK_JobUserJobDomainJoins_JobDomains_JobDomainId",
                        column: x => x.JobDomainId,
                        principalTable: "JobDomains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobUserJobDomainJoins_JobUsers_JobUserId",
                        column: x => x.JobUserId,
                        principalTable: "JobUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobUserJobDomainJoins_JobDomainId",
                table: "JobUserJobDomainJoins",
                column: "JobDomainId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobUserJobDomainJoins");

            migrationBuilder.AddColumn<Guid>(
                name: "JobUserEntityId",
                table: "JobDomains",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobDomains_JobUserEntityId",
                table: "JobDomains",
                column: "JobUserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobDomains_JobUsers_JobUserEntityId",
                table: "JobDomains",
                column: "JobUserEntityId",
                principalTable: "JobUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

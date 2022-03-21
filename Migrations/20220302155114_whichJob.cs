using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExplorJobAPI.Migrations
{
    public partial class whichJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "JobUserEntityId",
                table: "Reviews",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_JobUserEntityId",
                table: "Reviews",
                column: "JobUserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_JobUsers_JobUserEntityId",
                table: "Reviews",
                column: "JobUserEntityId",
                principalTable: "JobUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_JobUsers_JobUserEntityId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_JobUserEntityId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "JobUserEntityId",
                table: "Reviews");
        }
    }
}

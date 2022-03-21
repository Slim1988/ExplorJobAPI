using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExplorJobAPI.Migrations
{
    public partial class reviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageProposal_Conversations_ConversationEntityId",
                table: "MessageProposal");

            migrationBuilder.DropIndex(
                name: "IX_MessageProposal_ConversationEntityId",
                table: "MessageProposal");

            migrationBuilder.DropColumn(
                name: "ConversationEntityId",
                table: "MessageProposal");

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    HasMet = table.Column<bool>(nullable: false),
                    MeetingQuality = table.Column<int>(nullable: false),
                    MeetingPlateform = table.Column<int>(nullable: false),
                    MeetingDuration = table.Column<int>(nullable: false),
                    MeetingCanellationReason = table.Column<int>(nullable: false),
                    MeetingCanellationReasonOther = table.Column<string>(nullable: true),
                    Recommendation = table.Column<int>(nullable: false),
                    OtherComment = table.Column<string>(nullable: true),
                    CommonId = table.Column<Guid>(nullable: false),
                    EmitterId = table.Column<Guid>(nullable: false),
                    ReceiverId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_EmitterId",
                        column: x => x.EmitterId,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageProposal_ConversationId",
                table: "MessageProposal",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_EmitterId",
                table: "Reviews",
                column: "EmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReceiverId",
                table: "Reviews",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageProposal_Conversations_ConversationId",
                table: "MessageProposal",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageProposal_Conversations_ConversationId",
                table: "MessageProposal");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_MessageProposal_ConversationId",
                table: "MessageProposal");

            migrationBuilder.AddColumn<Guid>(
                name: "ConversationEntityId",
                table: "MessageProposal",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessageProposal_ConversationEntityId",
                table: "MessageProposal",
                column: "ConversationEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageProposal_Conversations_ConversationEntityId",
                table: "MessageProposal",
                column: "ConversationEntityId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace ExplorJobAPI.Migrations
{
    public partial class reviews_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageProposal_Conversations_ConversationId",
                table: "MessageProposal");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageProposal_Conversations_ConversationId",
                table: "MessageProposal",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageProposal_Conversations_ConversationId",
                table: "MessageProposal");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageProposal_Conversations_ConversationId",
                table: "MessageProposal",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

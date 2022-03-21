using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExplorJobAPI.Migrations
{
    public partial class renameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Conversations_ConversationId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Users_EmitterId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Users_ReceiverId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProposalAppointmentEntity_Appointments_MessageProposalId",
                table: "ProposalAppointmentEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_ConversationId",
                table: "Appointments");

            migrationBuilder.RenameTable(
                name: "Appointments",
                newName: "MessageProposal");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_ReceiverId",
                table: "MessageProposal",
                newName: "IX_MessageProposal_ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_EmitterId",
                table: "MessageProposal",
                newName: "IX_MessageProposal_EmitterId");

            migrationBuilder.AddColumn<Guid>(
                name: "ConversationEntityId",
                table: "MessageProposal",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageProposal",
                table: "MessageProposal",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_MessageProposal_Users_EmitterId",
                table: "MessageProposal",
                column: "EmitterId",
                principalTable: "Users",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageProposal_Users_ReceiverId",
                table: "MessageProposal",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProposalAppointmentEntity_MessageProposal_MessageProposalId",
                table: "ProposalAppointmentEntity",
                column: "MessageProposalId",
                principalTable: "MessageProposal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageProposal_Conversations_ConversationEntityId",
                table: "MessageProposal");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageProposal_Users_EmitterId",
                table: "MessageProposal");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageProposal_Users_ReceiverId",
                table: "MessageProposal");

            migrationBuilder.DropForeignKey(
                name: "FK_ProposalAppointmentEntity_MessageProposal_MessageProposalId",
                table: "ProposalAppointmentEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageProposal",
                table: "MessageProposal");

            migrationBuilder.DropIndex(
                name: "IX_MessageProposal_ConversationEntityId",
                table: "MessageProposal");

            migrationBuilder.DropColumn(
                name: "ConversationEntityId",
                table: "MessageProposal");

            migrationBuilder.RenameTable(
                name: "MessageProposal",
                newName: "Appointments");

            migrationBuilder.RenameIndex(
                name: "IX_MessageProposal_ReceiverId",
                table: "Appointments",
                newName: "IX_Appointments_ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageProposal_EmitterId",
                table: "Appointments",
                newName: "IX_Appointments_EmitterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ConversationId",
                table: "Appointments",
                column: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Conversations_ConversationId",
                table: "Appointments",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_EmitterId",
                table: "Appointments",
                column: "EmitterId",
                principalTable: "Users",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_ReceiverId",
                table: "Appointments",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProposalAppointmentEntity_Appointments_MessageProposalId",
                table: "ProposalAppointmentEntity",
                column: "MessageProposalId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

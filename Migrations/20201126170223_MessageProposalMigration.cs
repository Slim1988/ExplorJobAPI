using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExplorJobAPI.Migrations
{
    public partial class MessageProposalMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ConversationId = table.Column<Guid>(nullable: false),
                    EmitterId = table.Column<Guid>(nullable: false),
                    ReceiverId = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(maxLength: 500, nullable: false),
                    Read = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_EmitterId",
                        column: x => x.EmitterId,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProposalAppointmentEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MessageProposalId = table.Column<Guid>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    ProposalStaus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposalAppointmentEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProposalAppointmentEntity_Appointments_MessageProposalId",
                        column: x => x.MessageProposalId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ConversationId",
                table: "Appointments",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_EmitterId",
                table: "Appointments",
                column: "EmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ReceiverId",
                table: "Appointments",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposalAppointmentEntity_MessageProposalId",
                table: "ProposalAppointmentEntity",
                column: "MessageProposalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProposalAppointmentEntity");

            migrationBuilder.DropTable(
                name: "Appointments");
        }
    }
}

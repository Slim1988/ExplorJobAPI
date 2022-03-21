using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExplorJobAPI.Migrations
{
    public partial class OffersAdjustements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferSubscriptionPeriods_OfferSubscriptions_OfferSubscripti~",
                table: "OfferSubscriptionPeriods");

            migrationBuilder.DropColumn(
                name: "OfferSubscriptionId",
                table: "OfferSubscriptionPeriods");

            migrationBuilder.AlterColumn<Guid>(
                name: "OfferSubscriptionEntityId",
                table: "OfferSubscriptionPeriods",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferSubscriptionPeriods_OfferSubscriptions_OfferSubscripti~",
                table: "OfferSubscriptionPeriods",
                column: "OfferSubscriptionEntityId",
                principalTable: "OfferSubscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferSubscriptionPeriods_OfferSubscriptions_OfferSubscripti~",
                table: "OfferSubscriptionPeriods");

            migrationBuilder.AlterColumn<Guid>(
                name: "OfferSubscriptionEntityId",
                table: "OfferSubscriptionPeriods",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "OfferSubscriptionId",
                table: "OfferSubscriptionPeriods",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_OfferSubscriptionPeriods_OfferSubscriptions_OfferSubscripti~",
                table: "OfferSubscriptionPeriods",
                column: "OfferSubscriptionEntityId",
                principalTable: "OfferSubscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

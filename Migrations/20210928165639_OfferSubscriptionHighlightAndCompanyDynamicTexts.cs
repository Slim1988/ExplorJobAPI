using Microsoft.EntityFrameworkCore.Migrations;

namespace ExplorJobAPI.Migrations
{
    public partial class OfferSubscriptionHighlightAndCompanyDynamicTexts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TextOurMission",
                table: "Companies",
                newName: "DynamicText3Content");

            migrationBuilder.RenameColumn(
                name: "TextOurCorporateCulture",
                table: "Companies",
                newName: "DynamicText2Content");

            migrationBuilder.RenameColumn(
                name: "TextOurAdvantages",
                table: "Companies",
                newName: "DynamicText1Content");

            migrationBuilder.AddColumn<string>(
                name: "HighlightMessage",
                table: "OfferSubscriptions",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DynamicText1Title",
                table: "Companies",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DynamicText2Title",
                table: "Companies",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DynamicText3Title",
                table: "Companies",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HighlightMessage",
                table: "OfferSubscriptions");

            migrationBuilder.DropColumn(
                name: "DynamicText1Title",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "DynamicText2Title",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "DynamicText3Title",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "DynamicText3Content",
                table: "Companies",
                newName: "TextOurMission");

            migrationBuilder.RenameColumn(
                name: "DynamicText2Content",
                table: "Companies",
                newName: "TextOurCorporateCulture");

            migrationBuilder.RenameColumn(
                name: "DynamicText1Content",
                table: "Companies",
                newName: "TextOurAdvantages");
        }
    }
}

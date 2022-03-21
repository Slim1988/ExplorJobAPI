using Microsoft.EntityFrameworkCore.Migrations;

namespace ExplorJobAPI.Migrations
{
    public partial class CompanyUpdateWithWidgetsHTML : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoordinatesLatitude",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CoordinatesLongitude",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "MediaYoutubeVideoUrl",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "CoordinatesMapWidgetHTML",
                table: "Companies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MediaYoutubeVideoWidgetHTML",
                table: "Companies",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoordinatesMapWidgetHTML",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "MediaYoutubeVideoWidgetHTML",
                table: "Companies");

            migrationBuilder.AddColumn<decimal>(
                name: "CoordinatesLatitude",
                table: "Companies",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CoordinatesLongitude",
                table: "Companies",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "MediaYoutubeVideoUrl",
                table: "Companies",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);
        }
    }
}

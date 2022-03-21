using Microsoft.EntityFrameworkCore.Migrations;

namespace ExplorJobAPI.Migrations
{
    public partial class CompanyEditWidgetHTMLtoEmbedUrls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoordinatesMapWidgetHTML",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "MediaYoutubeVideoWidgetHTML",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "CoordinatesMapEmbedUrl",
                table: "Companies",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MediaYoutubeVideoEmbedUrl",
                table: "Companies",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoordinatesMapEmbedUrl",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "MediaYoutubeVideoEmbedUrl",
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
    }
}

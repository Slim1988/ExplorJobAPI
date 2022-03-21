using Microsoft.EntityFrameworkCore.Migrations;

namespace ExplorJobAPI.Migrations
{
    public partial class JobUserCompanyAndPresentation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "JobUsers",
                maxLength: 175,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Presentation",
                table: "JobUsers",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "JobUsers");

            migrationBuilder.DropColumn(
                name: "Presentation",
                table: "JobUsers");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExplorJobAPI.Migrations
{
    public partial class CompaniesAndOffers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserContactInformationJoins");

            migrationBuilder.DropColumn(
                name: "SkypeId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    ActivityArea = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    DynamicField1Title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    DynamicField1Content = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    DynamicField2Title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    DynamicField2Content = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    WebsiteUrl = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    SocialNetworkFacebookUrl = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SocialNetworkTwitterUrl = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SocialNetworkInstagramUrl = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SocialNetworkLinkedInUrl = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SocialNetworkYoutubeUrl = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MediaLogoUrl = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    MediaBannerUrl = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    MediaPhotoUrl1 = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    MediaPhotoUrl2 = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    MediaPhotoUrl3 = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    MediaPhotoUrl4 = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    MediaPhotoUrl5 = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    MediaPhotoUrl6 = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    MediaYoutubeVideoUrl = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    MediaFooterPhotoUrl = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CoordinatesLatitude = table.Column<decimal>(type: "numeric", nullable: false),
                    CoordinatesLongitude = table.Column<decimal>(type: "numeric", nullable: false),
                    TextCompanyCatchPhrase = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    TextPresentation = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    TextProfilesSought = table.Column<string>(type: "character varying(900)", maxLength: 900, nullable: false),
                    TextJobsCatchPhrase = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TextOurMission = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TextOurCorporateCulture = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TextOurAdvantages = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TextFooterPhrase = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    TextCareerWebsiteWording = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KeywordLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Keywords = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfferSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    TypeId = table.Column<string>(type: "text", nullable: false),
                    TypeLabel = table.Column<string>(type: "text", nullable: false),
                    LogoUrl = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Message = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferSubscriptions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfferSubscriptionPeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OfferSubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    End = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    OfferSubscriptionEntityId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferSubscriptionPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferSubscriptionPeriods_OfferSubscriptions_OfferSubscripti~",
                        column: x => x.OfferSubscriptionEntityId,
                        principalTable: "OfferSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Name",
                table: "Companies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KeywordLists_Name",
                table: "KeywordLists",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfferSubscriptionPeriods_OfferSubscriptionEntityId",
                table: "OfferSubscriptionPeriods",
                column: "OfferSubscriptionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferSubscriptions_CompanyId",
                table: "OfferSubscriptions",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeywordLists");

            migrationBuilder.DropTable(
                name: "OfferSubscriptionPeriods");

            migrationBuilder.DropTable(
                name: "OfferSubscriptions");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "SkypeId",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserContactInformationJoins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserContactInformationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContactInformationJoins", x => new { x.UserId, x.UserContactInformationId });
                    table.ForeignKey(
                        name: "FK_UserContactInformationJoins_UserContactInformations_UserCon~",
                        column: x => x.UserContactInformationId,
                        principalTable: "UserContactInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserContactInformationJoins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserContactInformationJoins_UserContactInformationId",
                table: "UserContactInformationJoins",
                column: "UserContactInformationId");
        }
    }
}

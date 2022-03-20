using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace netProject.Data.Migrations.Portfolio
{
    public partial class SecondUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    languageId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    languageTitle = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.languageId);
                });

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    programsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    programsTitle = table.Column<string>(type: "TEXT", nullable: false),
                    programsSchool = table.Column<string>(type: "TEXT", nullable: false),
                    programsDegree = table.Column<string>(type: "TEXT", nullable: true),
                    programsStartdate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    programsEnddate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.programsId);
                });

            migrationBuilder.CreateTable(
                name: "Website",
                columns: table => new
                {
                    websiteId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    websiteTitle = table.Column<string>(type: "TEXT", nullable: false),
                    websiteDescription = table.Column<string>(type: "TEXT", nullable: false),
                    websiteUrl = table.Column<string>(type: "TEXT", nullable: false),
                    websitePicture = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website", x => x.websiteId);
                });

            migrationBuilder.CreateTable(
                name: "Framework",
                columns: table => new
                {
                    frameworkId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    frameworkTitle = table.Column<string>(type: "TEXT", nullable: false),
                    languageId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Framework", x => x.frameworkId);
                    table.ForeignKey(
                        name: "FK_Framework_Language_languageId",
                        column: x => x.languageId,
                        principalTable: "Language",
                        principalColumn: "languageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    courseId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    courseTitle = table.Column<string>(type: "TEXT", nullable: false),
                    courseStartdate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    courseEnddate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    programsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.courseId);
                    table.ForeignKey(
                        name: "FK_Course_Programs_programsId",
                        column: x => x.programsId,
                        principalTable: "Programs",
                        principalColumn: "programsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebsiteLanguage",
                columns: table => new
                {
                    languageId = table.Column<int>(type: "INTEGER", nullable: false),
                    websiteId = table.Column<int>(type: "INTEGER", nullable: false),
                    WebsiteLanguageId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteLanguage", x => new { x.websiteId, x.languageId });
                    table.ForeignKey(
                        name: "FK_WebsiteLanguage_Language_languageId",
                        column: x => x.languageId,
                        principalTable: "Language",
                        principalColumn: "languageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WebsiteLanguage_Website_websiteId",
                        column: x => x.websiteId,
                        principalTable: "Website",
                        principalColumn: "websiteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebsiteFramework",
                columns: table => new
                {
                    frameworkId = table.Column<int>(type: "INTEGER", nullable: false),
                    websiteId = table.Column<int>(type: "INTEGER", nullable: false),
                    WebsiteFrameworkId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteFramework", x => new { x.websiteId, x.frameworkId });
                    table.ForeignKey(
                        name: "FK_WebsiteFramework_Framework_frameworkId",
                        column: x => x.frameworkId,
                        principalTable: "Framework",
                        principalColumn: "frameworkId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WebsiteFramework_Website_websiteId",
                        column: x => x.websiteId,
                        principalTable: "Website",
                        principalColumn: "websiteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Course_programsId",
                table: "Course",
                column: "programsId");

            migrationBuilder.CreateIndex(
                name: "IX_Framework_languageId",
                table: "Framework",
                column: "languageId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteFramework_frameworkId",
                table: "WebsiteFramework",
                column: "frameworkId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteLanguage_languageId",
                table: "WebsiteLanguage",
                column: "languageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "WebsiteFramework");

            migrationBuilder.DropTable(
                name: "WebsiteLanguage");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropTable(
                name: "Framework");

            migrationBuilder.DropTable(
                name: "Website");

            migrationBuilder.DropTable(
                name: "Language");
        }
    }
}

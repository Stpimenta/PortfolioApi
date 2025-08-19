using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioApi.Migrations
{
    /// <inheritdoc />
    public partial class addprojectinusertechprogress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectUserTechProgress",
                columns: table => new
                {
                    ProjectsId = table.Column<int>(type: "integer", nullable: false),
                    UserTechProgressesUserId = table.Column<int>(type: "integer", nullable: false),
                    UserTechProgressesTechId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUserTechProgress", x => new { x.ProjectsId, x.UserTechProgressesUserId, x.UserTechProgressesTechId });
                    table.ForeignKey(
                        name: "FK_ProjectUserTechProgress_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUserTechProgress_UserTechProgress_UserTechProgresses~",
                        columns: x => new { x.UserTechProgressesUserId, x.UserTechProgressesTechId },
                        principalTable: "UserTechProgress",
                        principalColumns: new[] { "UserId", "TechId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserTechProgress_UserTechProgressesUserId_UserTechPr~",
                table: "ProjectUserTechProgress",
                columns: new[] { "UserTechProgressesUserId", "UserTechProgressesTechId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectUserTechProgress");
        }
    }
}

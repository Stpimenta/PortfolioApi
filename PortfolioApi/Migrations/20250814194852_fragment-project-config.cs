using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioApi.Migrations
{
    /// <inheritdoc />
    public partial class fragmentprojectconfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "documentConfig_Documents",
                table: "Projects",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "downloadConfig_Plataforms",
                table: "Projects",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "downloadConfig_Steps",
                table: "Projects",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "gitConfig_Gits",
                table: "Projects",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "documentConfig_Documents",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "downloadConfig_Plataforms",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "downloadConfig_Steps",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "gitConfig_Gits",
                table: "Projects");
        }
    }
}

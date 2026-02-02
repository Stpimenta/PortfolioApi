using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioApi.Migrations
{
    /// <inheritdoc />
    public partial class fixprojectconfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Download",
                table: "Projects");

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

            migrationBuilder.RenameColumn(
                name: "Git",
                table: "Projects",
                newName: "Config");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Config",
                table: "Projects",
                newName: "Git");

            migrationBuilder.AddColumn<string>(
                name: "Download",
                table: "Projects",
                type: "text",
                nullable: true);

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
    }
}

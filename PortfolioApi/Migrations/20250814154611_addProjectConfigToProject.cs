using Microsoft.EntityFrameworkCore.Migrations;
using PortfolioApi.Domain.ValueObjects;

#nullable disable

namespace PortfolioApi.Migrations
{
    /// <inheritdoc />
    public partial class addProjectConfigToProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Config",
                table: "Projects");

          
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectConfig",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "Config",
                table: "Projects",
                type: "text",
                nullable: true);
        }
    }
}

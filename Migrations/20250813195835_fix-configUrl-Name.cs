using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioApi.Migrations
{
    /// <inheritdoc />
    public partial class fixconfigUrlName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ConfigUrl",
                table: "Users",
                newName: "Config");

            migrationBuilder.RenameColumn(
                name: "ConfigUrl",
                table: "Projects",
                newName: "Config");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Config",
                table: "Users",
                newName: "ConfigUrl");

            migrationBuilder.RenameColumn(
                name: "Config",
                table: "Projects",
                newName: "ConfigUrl");
        }
    }
}

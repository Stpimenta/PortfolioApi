using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioApi.Migrations
{
    /// <inheritdoc />
    public partial class fixdeleteicon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Technologies_Icons_IconId",
                table: "Technologies");

            migrationBuilder.AddForeignKey(
                name: "FK_Technologies_Icons_IconId",
                table: "Technologies",
                column: "IconId",
                principalTable: "Icons",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Technologies_Icons_IconId",
                table: "Technologies");

            migrationBuilder.AddForeignKey(
                name: "FK_Technologies_Icons_IconId",
                table: "Technologies",
                column: "IconId",
                principalTable: "Icons",
                principalColumn: "Id");
        }
    }
}

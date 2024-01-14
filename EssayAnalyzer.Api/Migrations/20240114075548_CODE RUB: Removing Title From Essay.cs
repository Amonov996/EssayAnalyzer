using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EssayAnalyzer.Api.Migrations
{
    /// <inheritdoc />
    public partial class CODERUBRemovingTitleFromEssay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Essays");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Essays",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

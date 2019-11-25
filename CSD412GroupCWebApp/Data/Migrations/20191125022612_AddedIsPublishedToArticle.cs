using Microsoft.EntityFrameworkCore.Migrations;

namespace CSD412GroupCWebApp.Data.Migrations
{
    public partial class AddedIsPublishedToArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Article",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Article");
        }
    }
}

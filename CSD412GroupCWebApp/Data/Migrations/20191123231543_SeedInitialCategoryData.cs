using Microsoft.EntityFrameworkCore.Migrations;

namespace CSD412GroupCWebApp.Data.Migrations
{
    public partial class SeedInitialCategoryData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "ArticleId", "Description", "Name" },
                values: new object[,]
                {
                    { 1L, null, "Articles about programming", "Programming" },
                    { 2L, null, "Articles about music", "Music" },
                    { 3L, null, "Articles about movies", "Movies" },
                    { 4L, null, "Articles about art", "Art" },
                    { 5L, null, "Articles about travel", "Travel" },
                    { 6L, null, "Articles about science", "Science" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 6L);
        }
    }
}

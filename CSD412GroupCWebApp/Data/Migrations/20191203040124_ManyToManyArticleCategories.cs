using Microsoft.EntityFrameworkCore.Migrations;

namespace CSD412GroupCWebApp.Data.Migrations
{
    public partial class ManyToManyArticleCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Article_ArticleId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_ArticleId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "Category");

            migrationBuilder.CreateTable(
                name: "ArticleCategories",
                columns: table => new
                {
                    ArticleId = table.Column<long>(nullable: false),
                    CategoryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCategories", x => new { x.ArticleId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ArticleCategories_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleCategories_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleCategories_CategoryId",
                table: "ArticleCategories",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleCategories");

            migrationBuilder.AddColumn<long>(
                name: "ArticleId",
                table: "Category",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_ArticleId",
                table: "Category",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Article_ArticleId",
                table: "Category",
                column: "ArticleId",
                principalTable: "Article",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

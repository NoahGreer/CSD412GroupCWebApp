using Microsoft.EntityFrameworkCore.Migrations;

namespace CSD412GroupCWebApp.Data.Migrations
{
    public partial class UpdateArticleModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_Category_CategoriesId",
                table: "Article");

            migrationBuilder.DropIndex(
                name: "IX_Article_CategoriesId",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "CategoriesId",
                table: "Article");

            migrationBuilder.AddColumn<long>(
                name: "ArticleId",
                table: "Category",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Article",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_ArticleId",
                table: "Category",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Article_AuthorId",
                table: "Article",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_AspNetUsers_AuthorId",
                table: "Article",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Article_ArticleId",
                table: "Category",
                column: "ArticleId",
                principalTable: "Article",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_AspNetUsers_AuthorId",
                table: "Article");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_Article_ArticleId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_ArticleId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Article_AuthorId",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Article");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Article",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CategoriesId",
                table: "Article",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Article_CategoriesId",
                table: "Article",
                column: "CategoriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_Category_CategoriesId",
                table: "Article",
                column: "CategoriesId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace CSD412GroupCWebApp.Data.Migrations
{
    public partial class RemoveBlogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_Blog_BlogId",
                table: "Article");

            migrationBuilder.DropTable(
                name: "Blog");

            migrationBuilder.DropIndex(
                name: "IX_Article_BlogId",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "Article");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BlogId",
                table: "Article",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Authors = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Article_BlogId",
                table: "Article",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_Blog_BlogId",
                table: "Article",
                column: "BlogId",
                principalTable: "Blog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

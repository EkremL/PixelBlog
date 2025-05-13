using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddBlogSaveEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogSave_Blogs_BlogId",
                table: "BlogSave");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogSave_Users_UserId",
                table: "BlogSave");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogSave",
                table: "BlogSave");

            migrationBuilder.DropIndex(
                name: "IX_BlogSave_UserId",
                table: "BlogSave");

            migrationBuilder.RenameTable(
                name: "BlogSave",
                newName: "BlogSaves");

            migrationBuilder.RenameIndex(
                name: "IX_BlogSave_BlogId",
                table: "BlogSaves",
                newName: "IX_BlogSaves_BlogId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "BlogSaves",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogSaves",
                table: "BlogSaves",
                columns: new[] { "UserId", "BlogId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BlogSaves_Blogs_BlogId",
                table: "BlogSaves",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogSaves_Users_UserId",
                table: "BlogSaves",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogSaves_Blogs_BlogId",
                table: "BlogSaves");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogSaves_Users_UserId",
                table: "BlogSaves");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogSaves",
                table: "BlogSaves");

            migrationBuilder.RenameTable(
                name: "BlogSaves",
                newName: "BlogSave");

            migrationBuilder.RenameIndex(
                name: "IX_BlogSaves_BlogId",
                table: "BlogSave",
                newName: "IX_BlogSave_BlogId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "BlogSave",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogSave",
                table: "BlogSave",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BlogSave_UserId",
                table: "BlogSave",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogSave_Blogs_BlogId",
                table: "BlogSave",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogSave_Users_UserId",
                table: "BlogSave",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_253505_Bekarev.API.Migrations
{
    /// <inheritdoc />
    public partial class migration_str2int : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animes_Categories_CategoryId1",
                table: "Animes");

            migrationBuilder.DropIndex(
                name: "IX_Animes_CategoryId1",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Animes");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Animes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animes_CategoryId",
                table: "Animes",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Animes_Categories_CategoryId",
                table: "Animes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animes_Categories_CategoryId",
                table: "Animes");

            migrationBuilder.DropIndex(
                name: "IX_Animes_CategoryId",
                table: "Animes");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "Animes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Animes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animes_CategoryId1",
                table: "Animes",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Animes_Categories_CategoryId1",
                table: "Animes",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}

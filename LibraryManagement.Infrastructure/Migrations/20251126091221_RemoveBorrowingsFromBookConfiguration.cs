using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBorrowingsFromBookConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrowings_Books_BookId1",
                table: "Borrowings");

            migrationBuilder.DropIndex(
                name: "IX_Borrowings_BookId1",
                table: "Borrowings");

            migrationBuilder.DropColumn(
                name: "BookId1",
                table: "Borrowings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BookId1",
                table: "Borrowings",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Borrowings_BookId1",
                table: "Borrowings",
                column: "BookId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowings_Books_BookId1",
                table: "Borrowings",
                column: "BookId1",
                principalTable: "Books",
                principalColumn: "BookId");
        }
    }
}

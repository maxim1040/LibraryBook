using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryBook.Migrations
{
    /// <inheritdoc />
    public partial class loanerUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_User_UserId",
                table: "Loans");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Loans",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_UserId1",
                table: "Loans",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_AspNetUsers_UserId",
                table: "Loans",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_User_UserId1",
                table: "Loans",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_AspNetUsers_UserId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_User_UserId1",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_UserId1",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Loans");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_User_UserId",
                table: "Loans",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}

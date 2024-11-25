using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeeNice.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserToApiary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Apiary",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Apiary_UserId",
                table: "Apiary",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Apiary_AspNetUsers_UserId",
                table: "Apiary",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apiary_AspNetUsers_UserId",
                table: "Apiary");

            migrationBuilder.DropIndex(
                name: "IX_Apiary_UserId",
                table: "Apiary");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Apiary");
        }
    }
}

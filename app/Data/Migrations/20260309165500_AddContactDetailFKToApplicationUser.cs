using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSD2600_CDEGP.Migrations
{
    /// <inheritdoc />
    public partial class AddContactDetailFKToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FkContactId",
                table: "AspNetUsers",
                type: "int",
                nullable: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FkContactId",
                table: "AspNetUsers",
                column: "FkContactId",
                unique: true,
                filter: "[FkContactId] IS NOT NULL"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ContactDetail_FkContactId",
                table: "AspNetUsers",
                column: "FkContactId",
                principalTable: "ContactDetail",
                principalColumn: "PkContactId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ContactDetail_FkContactId",
                table: "AspNetUsers"
            );

            migrationBuilder.DropIndex(name: "IX_AspNetUsers_FkContactId", table: "AspNetUsers");

            migrationBuilder.DropColumn(name: "FkContactId", table: "AspNetUsers");
        }
    }
}

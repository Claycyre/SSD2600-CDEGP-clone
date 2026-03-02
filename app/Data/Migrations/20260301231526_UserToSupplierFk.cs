using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSD2600_CDEGP.Migrations
{
    /// <inheritdoc />
    public partial class UserToSupplierFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FkSupplierId",
                table: "AspNetUsers",
                type: "int",
                nullable: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FkSupplierId",
                table: "AspNetUsers",
                column: "FkSupplierId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Suppliers_FkSupplierId",
                table: "AspNetUsers",
                column: "FkSupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Suppliers_FkSupplierId",
                table: "AspNetUsers"
            );

            migrationBuilder.DropIndex(name: "IX_AspNetUsers_FkSupplierId", table: "AspNetUsers");

            migrationBuilder.DropColumn(name: "FkSupplierId", table: "AspNetUsers");
        }
    }
}

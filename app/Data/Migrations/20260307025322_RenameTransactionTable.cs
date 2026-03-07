using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSD2600_CDEGP.Migrations
{
    /// <inheritdoc />
    public partial class RenameTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrder_Transaction_FkTransactionId",
                table: "ProductOrder"
            );

            migrationBuilder.DropPrimaryKey(name: "PK_Transaction", table: "Transaction");

            migrationBuilder.RenameTable(name: "Transaction", newName: "OrderTransaction");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderTransaction",
                table: "OrderTransaction",
                column: "PkTransactionId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrder_OrderTransaction_FkTransactionId",
                table: "ProductOrder",
                column: "FkTransactionId",
                principalTable: "OrderTransaction",
                principalColumn: "PkTransactionId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrder_OrderTransaction_FkTransactionId",
                table: "ProductOrder"
            );

            migrationBuilder.DropPrimaryKey(name: "PK_OrderTransaction", table: "OrderTransaction");

            migrationBuilder.RenameTable(name: "OrderTransaction", newName: "Transaction");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction",
                column: "PkTransactionId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrder_Transaction_FkTransactionId",
                table: "ProductOrder",
                column: "FkTransactionId",
                principalTable: "Transaction",
                principalColumn: "PkTransactionId"
            );
        }
    }
}

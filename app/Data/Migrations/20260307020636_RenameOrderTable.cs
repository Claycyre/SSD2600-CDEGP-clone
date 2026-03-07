using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSD2600_CDEGP.Migrations
{
    /// <inheritdoc />
    public partial class RenameOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Order_AspNetUsers_FkUserId", table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Transaction_FkTransactionId",
                table: "Order"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLineItem_Order_FkOrderId",
                table: "OrderLineItem"
            );

            migrationBuilder.DropPrimaryKey(name: "PK_Order", table: "Order");

            migrationBuilder.RenameTable(name: "Order", newName: "ProductOrder");

            migrationBuilder.RenameIndex(
                name: "IX_Order_FkUserId",
                table: "ProductOrder",
                newName: "IX_ProductOrder_FkUserId"
            );

            migrationBuilder.RenameIndex(
                name: "IX_Order_FkTransactionId",
                table: "ProductOrder",
                newName: "IX_ProductOrder_FkTransactionId"
            );

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductOrder",
                table: "ProductOrder",
                column: "PkOrderId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLineItem_ProductOrder_FkOrderId",
                table: "OrderLineItem",
                column: "FkOrderId",
                principalTable: "ProductOrder",
                principalColumn: "PkOrderId",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrder_AspNetUsers_FkUserId",
                table: "ProductOrder",
                column: "FkUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrder_Transaction_FkTransactionId",
                table: "ProductOrder",
                column: "FkTransactionId",
                principalTable: "Transaction",
                principalColumn: "PkTransactionId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLineItem_ProductOrder_FkOrderId",
                table: "OrderLineItem"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrder_AspNetUsers_FkUserId",
                table: "ProductOrder"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrder_Transaction_FkTransactionId",
                table: "ProductOrder"
            );

            migrationBuilder.DropPrimaryKey(name: "PK_ProductOrder", table: "ProductOrder");

            migrationBuilder.RenameTable(name: "ProductOrder", newName: "Order");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOrder_FkUserId",
                table: "Order",
                newName: "IX_Order_FkUserId"
            );

            migrationBuilder.RenameIndex(
                name: "IX_ProductOrder_FkTransactionId",
                table: "Order",
                newName: "IX_Order_FkTransactionId"
            );

            migrationBuilder.AddPrimaryKey(name: "PK_Order", table: "Order", column: "PkOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_FkUserId",
                table: "Order",
                column: "FkUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Transaction_FkTransactionId",
                table: "Order",
                column: "FkTransactionId",
                principalTable: "Transaction",
                principalColumn: "PkTransactionId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLineItem_Order_FkOrderId",
                table: "OrderLineItem",
                column: "FkOrderId",
                principalTable: "Order",
                principalColumn: "PkOrderId",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}

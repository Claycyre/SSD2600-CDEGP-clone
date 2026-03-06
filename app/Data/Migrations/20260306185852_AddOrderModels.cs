using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSD2600_CDEGP.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    PkTransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GatewayTransactionId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GatewayName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Subtotal = table.Column<double>(type: "float", nullable: false),
                    CombinedTax = table.Column<double>(type: "float", nullable: true),
                    Total = table.Column<double>(type: "float", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.PkTransactionId);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    PkOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusTimeframe = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    FkUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    FkTransactionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.PkOrderId);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_FkUserId",
                        column: x => x.FkUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Transaction_FkTransactionId",
                        column: x => x.FkTransactionId,
                        principalTable: "Transaction",
                        principalColumn: "PkTransactionId");
                });

            migrationBuilder.CreateTable(
                name: "OrderLineItem",
                columns: table => new
                {
                    PkOrderLineItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkOrderId = table.Column<int>(type: "int", nullable: false),
                    FkProductSKU = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLineItem", x => x.PkOrderLineItemId);
                    table.ForeignKey(
                        name: "FK_OrderLineItem_Order_FkOrderId",
                        column: x => x.FkOrderId,
                        principalTable: "Order",
                        principalColumn: "PkOrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderLineItem_Product_FkProductSKU",
                        column: x => x.FkProductSKU,
                        principalTable: "Product",
                        principalColumn: "PkSKU",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_FkTransactionId",
                table: "Order",
                column: "FkTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_FkUserId",
                table: "Order",
                column: "FkUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLineItem_FkOrderId",
                table: "OrderLineItem",
                column: "FkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLineItem_FkProductSKU",
                table: "OrderLineItem",
                column: "FkProductSKU");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderLineItem");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Transaction");
        }
    }
}

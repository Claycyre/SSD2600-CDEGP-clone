using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSD2600_CDEGP.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderLineItem",
                table: "OrderLineItem");

            migrationBuilder.DropIndex(
                name: "IX_OrderLineItem_FkOrderId",
                table: "OrderLineItem");

            migrationBuilder.DropColumn(
                name: "PkOrderLineItemId",
                table: "OrderLineItem");

            migrationBuilder.DropColumn(
                name: "StatusTimeframe",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Order");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderLineItem",
                table: "OrderLineItem",
                columns: new[] { "FkOrderId", "FkProductSKU" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderLineItem",
                table: "OrderLineItem");

            migrationBuilder.AddColumn<int>(
                name: "PkOrderLineItemId",
                table: "OrderLineItem",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "StatusTimeframe",
                table: "Order",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "Order",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderLineItem",
                table: "OrderLineItem",
                column: "PkOrderLineItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLineItem_FkOrderId",
                table: "OrderLineItem",
                column: "FkOrderId");
        }
    }
}

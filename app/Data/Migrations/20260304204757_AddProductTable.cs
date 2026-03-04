using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSD2600_CDEGP.Migrations
{
    /// <inheritdoc />
    public partial class AddProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreferredCurrencyCode",
                table: "AspNetUsers",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    PkSKU = table
                        .Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(
                        type: "nvarchar(70)",
                        maxLength: 70,
                        nullable: false
                    ),
                    ShortName = table.Column<string>(
                        type: "nvarchar(30)",
                        maxLength: 30,
                        nullable: true
                    ),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Media = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    UnitPrice = table.Column<double>(type: "float", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    Attributes = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    AtomicNumber = table.Column<int>(type: "int", nullable: true),
                    StateOfMatter = table.Column<string>(
                        type: "nvarchar(50)",
                        maxLength: 50,
                        nullable: false
                    ),
                    ProductType = table.Column<string>(
                        type: "nvarchar(50)",
                        maxLength: 50,
                        nullable: false
                    ),
                    ProductSubtype = table.Column<string>(
                        type: "nvarchar(60)",
                        maxLength: 60,
                        nullable: true
                    ),
                    HalfLife = table.Column<string>(
                        type: "nvarchar(30)",
                        maxLength: 30,
                        nullable: true
                    ),
                    Purity = table.Column<string>(
                        type: "nvarchar(60)",
                        maxLength: 60,
                        nullable: true
                    ),
                    SpecificActivity = table.Column<string>(
                        type: "nvarchar(80)",
                        maxLength: 80,
                        nullable: true
                    ),
                    FkSupplierId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.PkSKU);
                    table.ForeignKey(
                        name: "FK_Product_Suppliers_FkSupplierId",
                        column: x => x.FkSupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Product_FkSupplierId",
                table: "Product",
                column: "FkSupplierId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Product");

            migrationBuilder.DropColumn(name: "PreferredCurrencyCode", table: "AspNetUsers");
        }
    }
}

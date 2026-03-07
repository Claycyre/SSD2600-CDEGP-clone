using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSD2600_CDEGP.Migrations
{
    /// <inheritdoc />
    public partial class AddContactDetailModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactDetail",
                columns: table => new
                {
                    PkContactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameFirst = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameLast = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AdministrativeArea = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactDetail", x => x.PkContactId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactDetail");
        }
    }
}

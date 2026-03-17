using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSD2600_CDEGP.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsForVerificationFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdminVerified",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisteredAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
            );

            migrationBuilder.AddColumn<bool>(
                name: "UserBanned",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.AddColumn<string>(
                name: "UserRole",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<bool>(
                name: "UserSuspended",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.AddColumn<bool>(
                name: "VerificationApproved",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.AddColumn<bool>(
                name: "VerificationSubmitted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.CreateTable(
                name: "AdminMessages",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkSenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FkRecipientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Subject = table.Column<string>(
                        type: "nvarchar(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminMessages_AspNetUsers_FkRecipientId",
                        column: x => x.FkRecipientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict
                    );
                    table.ForeignKey(
                        name: "FK_AdminMessages_AspNetUsers_FkSenderId",
                        column: x => x.FkSenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_AdminMessages_FkRecipientId",
                table: "AdminMessages",
                column: "FkRecipientId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_AdminMessages_FkSenderId",
                table: "AdminMessages",
                column: "FkSenderId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "AdminMessages");

            migrationBuilder.DropColumn(name: "IsAdminVerified", table: "Product");

            migrationBuilder.DropColumn(name: "RegisteredAt", table: "AspNetUsers");

            migrationBuilder.DropColumn(name: "UserBanned", table: "AspNetUsers");

            migrationBuilder.DropColumn(name: "UserRole", table: "AspNetUsers");

            migrationBuilder.DropColumn(name: "UserSuspended", table: "AspNetUsers");

            migrationBuilder.DropColumn(name: "VerificationApproved", table: "AspNetUsers");

            migrationBuilder.DropColumn(name: "VerificationSubmitted", table: "AspNetUsers");
        }
    }
}

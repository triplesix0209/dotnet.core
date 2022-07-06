using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Infrastructure.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdateDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_CreateDateTime",
                table: "Account",
                column: "CreateDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Account_CreatorId",
                table: "Account",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_IsDeleted",
                table: "Account",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Account_Name",
                table: "Account",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Account_UpdateDateTime",
                table: "Account",
                column: "UpdateDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Account_UpdatorId",
                table: "Account",
                column: "UpdatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}

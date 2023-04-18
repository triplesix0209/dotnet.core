using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "Id", "Code", "CreateDateTime", "CreatorId", "IsDeleted", "Name", "UpdateDateTime", "UpdatorId" },
                values: new object[] { new Guid("653dc4d4-ca05-45ac-83cd-e98fa91b890f"), null, null, null, false, "Root", null, null });

            migrationBuilder.InsertData(
                table: "Setting",
                columns: new[] { "Id", "Code", "CreateDateTime", "CreatorId", "Description", "IsDeleted", "UpdateDateTime", "UpdatorId", "Value" },
                values: new object[] { new Guid("22ccd8c5-6656-48f5-a73e-8d75895c9adc"), "SessionLifetime", null, null, "Thời gian sống của session (phút)", false, null, null, "120" });

            migrationBuilder.CreateIndex(
                name: "IX_Account_Code",
                table: "Account",
                column: "Code");

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

            migrationBuilder.CreateIndex(
                name: "IX_Setting_Code",
                table: "Setting",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Setting_CreateDateTime",
                table: "Setting",
                column: "CreateDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_CreatorId",
                table: "Setting",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_IsDeleted",
                table: "Setting",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_UpdateDateTime",
                table: "Setting",
                column: "UpdateDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_UpdatorId",
                table: "Setting",
                column: "UpdatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Setting");
        }
    }
}

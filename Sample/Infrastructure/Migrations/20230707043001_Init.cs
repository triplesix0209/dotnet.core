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
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Mã số"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Tên gọi"),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Thời gian khởi tạo"),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Id người tạo"),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Thời gian chỉnh sửa cuối"),
                    UpdatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Id người chỉnh sửa"),
                    DeleteAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Thời gian xóa")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Mã"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Giá trị"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Mô tả"),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Thời gian khởi tạo"),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Id người tạo"),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Thời gian chỉnh sửa cuối"),
                    UpdatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Id người chỉnh sửa"),
                    DeleteAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Thời gian xóa")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "Id", "Code", "CreateAt", "CreatorId", "DeleteAt", "Name", "UpdateAt", "UpdatorId" },
                values: new object[] { new Guid("653dc4d4-ca05-45ac-83cd-e98fa91b890f"), "root", null, null, null, "Root", null, null });

            migrationBuilder.InsertData(
                table: "Setting",
                columns: new[] { "Id", "Code", "CreateAt", "CreatorId", "DeleteAt", "Description", "UpdateAt", "UpdatorId", "Value" },
                values: new object[] { new Guid("22ccd8c5-6656-48f5-a73e-8d75895c9adc"), "SessionLifetime", null, null, null, "Thời gian sống của session (phút)", null, null, "240" });

            migrationBuilder.CreateIndex(
                name: "IX_Account_Code",
                table: "Account",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_CreateAt",
                table: "Account",
                column: "CreateAt");

            migrationBuilder.CreateIndex(
                name: "IX_Account_CreatorId",
                table: "Account",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_DeleteAt",
                table: "Account",
                column: "DeleteAt");

            migrationBuilder.CreateIndex(
                name: "IX_Account_Name",
                table: "Account",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Account_UpdateAt",
                table: "Account",
                column: "UpdateAt");

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
                name: "IX_Setting_CreateAt",
                table: "Setting",
                column: "CreateAt");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_CreatorId",
                table: "Setting",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_DeleteAt",
                table: "Setting",
                column: "DeleteAt");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_UpdateAt",
                table: "Setting",
                column: "UpdateAt");

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

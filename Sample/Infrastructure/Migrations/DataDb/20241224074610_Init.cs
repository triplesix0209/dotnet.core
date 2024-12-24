using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Infrastructure.Migrations.DataDb
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Test",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Mã số"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Tên gọi"),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Thời gian khởi tạo"),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Id người tạo"),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Thời gian chỉnh sửa cuối"),
                    UpdatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Id người chỉnh sửa"),
                    DeleteAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Thời gian xóa")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.Id);
                },
                comment: "Test");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Test");
        }
    }
}

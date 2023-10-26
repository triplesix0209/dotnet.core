using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Site",
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
                    table.PrimaryKey("PK_Site", x => x.Id);
                },
                comment: "Chi nhánh");

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Mã số"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Tên gọi"),
                    SiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Id chi nhánh"),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Thời gian khởi tạo"),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Id người tạo"),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Thời gian chỉnh sửa cuối"),
                    UpdatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Id người chỉnh sửa"),
                    DeleteAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Thời gian xóa")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_Site_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Site",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Tài khoản");

            migrationBuilder.InsertData(
                table: "Site",
                columns: new[] { "Id", "Code", "CreateAt", "CreatorId", "DeleteAt", "Name", "UpdateAt", "UpdatorId" },
                values: new object[,]
                {
                    { new Guid("3e08cf2e-d8a2-49b5-8663-fa31f0cdd168"), "H002", null, null, null, "Quận 6", null, null },
                    { new Guid("7a2ed7c2-e6f7-48c1-a86a-aa701aee1e22"), "H001", null, null, null, "Quận 5", null, null }
                });

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "Id", "Code", "CreateAt", "CreatorId", "DeleteAt", "Name", "SiteId", "UpdateAt", "UpdatorId" },
                values: new object[,]
                {
                    { new Guid("653dc4d4-ca05-45ac-83cd-e98fa91b890f"), "EM001", null, null, null, "Nhân Viên 1", new Guid("7a2ed7c2-e6f7-48c1-a86a-aa701aee1e22"), null, null },
                    { new Guid("6f6e615e-feeb-40b5-b53c-7f9056082d36"), "EM002", null, null, null, "Nhân Viên 2", new Guid("7a2ed7c2-e6f7-48c1-a86a-aa701aee1e22"), null, null },
                    { new Guid("72b44a93-defc-4e24-a466-0d0d36b3669c"), "EM003", null, null, null, "Nhân Viên 3", new Guid("3e08cf2e-d8a2-49b5-8663-fa31f0cdd168"), null, null }
                });

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
                name: "IX_Account_SiteId",
                table: "Account",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_UpdateAt",
                table: "Account",
                column: "UpdateAt");

            migrationBuilder.CreateIndex(
                name: "IX_Account_UpdatorId",
                table: "Account",
                column: "UpdatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Site_Code",
                table: "Site",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Site_CreateAt",
                table: "Site",
                column: "CreateAt");

            migrationBuilder.CreateIndex(
                name: "IX_Site_CreatorId",
                table: "Site",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Site_DeleteAt",
                table: "Site",
                column: "DeleteAt");

            migrationBuilder.CreateIndex(
                name: "IX_Site_Name",
                table: "Site",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Site_UpdateAt",
                table: "Site",
                column: "UpdateAt");

            migrationBuilder.CreateIndex(
                name: "IX_Site_UpdatorId",
                table: "Site",
                column: "UpdatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Site");
        }
    }
}

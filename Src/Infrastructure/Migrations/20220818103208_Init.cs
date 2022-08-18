using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Infrastructure.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ObjectLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ObjectType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ObjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectLog", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoryName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Code);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PermissionGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    HierarchyLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionGroup_PermissionGroup_ParentId",
                        column: x => x.ParentId,
                        principalTable: "PermissionGroup",
                        principalColumn: "Id");
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

            migrationBuilder.CreateTable(
                name: "ObjectLogField",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    FieldName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OldValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NewValue = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LogId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectLogField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectLogField_ObjectLog_LogId",
                        column: x => x.LogId,
                        principalTable: "ObjectLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AvatarLink = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccessLevel = table.Column<int>(type: "int", nullable: false),
                    PermissionGroupId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
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
                    table.ForeignKey(
                        name: "FK_Account_PermissionGroup_PermissionGroupId",
                        column: x => x.PermissionGroupId,
                        principalTable: "PermissionGroup",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PermissionValue",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GroupId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Value = table.Column<int>(type: "int", nullable: false),
                    ActualValue = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionValue", x => new { x.Code, x.GroupId });
                    table.ForeignKey(
                        name: "FK_PermissionValue_Permission_Code",
                        column: x => x.Code,
                        principalTable: "Permission",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionValue_PermissionGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "PermissionGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccountAuth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Username = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HashPasswordKey = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HashedPassword = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
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
                    table.PrimaryKey("PK_AccountAuth", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountAuth_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccountSession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpiryDatetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountSession_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "Id", "AccessLevel", "AvatarLink", "Code", "CreateDateTime", "CreatorId", "IsDeleted", "Name", "PermissionGroupId", "UpdateDateTime", "UpdatorId" },
                values: new object[] { new Guid("653dc4d4-ca05-45ac-83cd-e98fa91b890f"), 0, null, null, null, null, true, "Root", null, null, null });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Code", "CategoryName", "Name" },
                values: new object[,]
                {
                    { "account.changelog", "Tài khoản", "Xem log tài khoản" },
                    { "account.create", "Tài khoản", "Tạo tài khoản" },
                    { "account.delete", "Tài khoản", "Xóa tài khoản" },
                    { "account.export", "Tài khoản", "Xuất tài khoản" },
                    { "account.read", "Tài khoản", "Đọc tài khoản" },
                    { "account.update", "Tài khoản", "Sửa tài khoản" },
                    { "permission.changelog", "Quyền", "Xem log quyền" },
                    { "permission.create", "Quyền", "Tạo quyền" },
                    { "permission.delete", "Quyền", "Xóa quyền" },
                    { "permission.export", "Quyền", "Xuất quyền" },
                    { "permission.read", "Quyền", "Đọc quyền" },
                    { "permission.update", "Quyền", "Sửa quyền" },
                    { "profile.update", "Thông tin cá nhân", "Sửa thông tin cá nhân" },
                    { "setting.changelog", "Thiết lập", "Xem log thiết lập" },
                    { "setting.export", "Thiết lập", "Xuất thiết lập" },
                    { "setting.read", "Thiết lập", "Đọc thiết lập" },
                    { "setting.update", "Thiết lập", "Sửa thiết lập" }
                });

            migrationBuilder.InsertData(
                table: "PermissionGroup",
                columns: new[] { "Id", "Code", "CreateDateTime", "CreatorId", "HierarchyLevel", "IsDeleted", "Name", "ParentId", "UpdateDateTime", "UpdatorId" },
                values: new object[] { new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), "Admin", null, null, 0, false, "Nhóm quyền quản trị", null, null, null });

            migrationBuilder.InsertData(
                table: "Setting",
                columns: new[] { "Id", "Code", "CreateDateTime", "CreatorId", "Description", "IsDeleted", "UpdateDateTime", "UpdatorId", "Value" },
                values: new object[] { new Guid("22ccd8c5-6656-48f5-a73e-8d75895c9adc"), "AccountRegisterAutoAccept", null, null, "Tự động duyệt các tài khoản mới đăng ký", false, null, null, "false" });

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "Id", "AccessLevel", "AvatarLink", "Code", "CreateDateTime", "CreatorId", "IsDeleted", "Name", "PermissionGroupId", "UpdateDateTime", "UpdatorId" },
                values: new object[] { new Guid("b81d0c90-3b91-44d4-bb00-95a5925fa5c6"), 1, null, null, null, null, false, "Admin", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), null, null });

            migrationBuilder.InsertData(
                table: "AccountAuth",
                columns: new[] { "Id", "AccountId", "Code", "CreateDateTime", "CreatorId", "HashPasswordKey", "HashedPassword", "IsDeleted", "UpdateDateTime", "UpdatorId", "Username" },
                values: new object[] { new Guid("653dc4d4-ca05-45ac-83cd-e98fa91b890f"), new Guid("653dc4d4-ca05-45ac-83cd-e98fa91b890f"), null, null, null, "8sBXJjPl1BaK1ppd0PNMB366NHhmAx", "F0DC6BFBD368CDD9D63FC264C8B76E9F", false, null, null, "root" });

            migrationBuilder.InsertData(
                table: "PermissionValue",
                columns: new[] { "Code", "GroupId", "ActualValue", "Value" },
                values: new object[,]
                {
                    { "account.changelog", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "account.create", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "account.delete", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "account.export", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "account.read", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "account.update", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "permission.changelog", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "permission.create", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "permission.delete", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "permission.export", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "permission.read", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "permission.update", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "profile.update", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "setting.changelog", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "setting.export", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "setting.read", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "setting.update", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 }
                });

            migrationBuilder.InsertData(
                table: "AccountAuth",
                columns: new[] { "Id", "AccountId", "Code", "CreateDateTime", "CreatorId", "HashPasswordKey", "HashedPassword", "IsDeleted", "UpdateDateTime", "UpdatorId", "Username" },
                values: new object[] { new Guid("b81d0c90-3b91-44d4-bb00-95a5925fa5c6"), new Guid("b81d0c90-3b91-44d4-bb00-95a5925fa5c6"), null, null, null, "xE8czZlAixQOJDQ0oR7PqlYJUcywj6", "73906ACDE3F2288B37120E99886A57D7", false, null, null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Account_AccessLevel",
                table: "Account",
                column: "AccessLevel");

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
                name: "IX_Account_PermissionGroupId",
                table: "Account",
                column: "PermissionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_UpdateDateTime",
                table: "Account",
                column: "UpdateDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Account_UpdatorId",
                table: "Account",
                column: "UpdatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountAuth_AccountId",
                table: "AccountAuth",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountAuth_Code",
                table: "AccountAuth",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_AccountAuth_CreateDateTime",
                table: "AccountAuth",
                column: "CreateDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_AccountAuth_CreatorId",
                table: "AccountAuth",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountAuth_IsDeleted",
                table: "AccountAuth",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AccountAuth_UpdateDateTime",
                table: "AccountAuth",
                column: "UpdateDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_AccountAuth_UpdatorId",
                table: "AccountAuth",
                column: "UpdatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountAuth_Username",
                table: "AccountAuth",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountSession_AccountId",
                table: "AccountSession",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountSession_Code",
                table: "AccountSession",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_AccountSession_CreateDateTime",
                table: "AccountSession",
                column: "CreateDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_AccountSession_CreatorId",
                table: "AccountSession",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountSession_ExpiryDatetime",
                table: "AccountSession",
                column: "ExpiryDatetime");

            migrationBuilder.CreateIndex(
                name: "IX_AccountSession_IsDeleted",
                table: "AccountSession",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AccountSession_UpdateDateTime",
                table: "AccountSession",
                column: "UpdateDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_AccountSession_UpdatorId",
                table: "AccountSession",
                column: "UpdatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectLogField_LogId",
                table: "ObjectLogField",
                column: "LogId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_CategoryName",
                table: "Permission",
                column: "CategoryName");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Code",
                table: "Permission",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Name",
                table: "Permission",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroup_Code",
                table: "PermissionGroup",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroup_CreateDateTime",
                table: "PermissionGroup",
                column: "CreateDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroup_CreatorId",
                table: "PermissionGroup",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroup_IsDeleted",
                table: "PermissionGroup",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroup_Name",
                table: "PermissionGroup",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroup_ParentId",
                table: "PermissionGroup",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroup_UpdateDateTime",
                table: "PermissionGroup",
                column: "UpdateDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroup_UpdatorId",
                table: "PermissionGroup",
                column: "UpdatorId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionValue_GroupId",
                table: "PermissionValue",
                column: "GroupId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountAuth");

            migrationBuilder.DropTable(
                name: "AccountSession");

            migrationBuilder.DropTable(
                name: "ObjectLogField");

            migrationBuilder.DropTable(
                name: "PermissionValue");

            migrationBuilder.DropTable(
                name: "Setting");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "ObjectLog");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "PermissionGroup");
        }
    }
}

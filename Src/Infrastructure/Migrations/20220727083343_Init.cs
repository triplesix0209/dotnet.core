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
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                    Code = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(255)", nullable: true)
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
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false)
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
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_CreateDateTime",
                table: "Setting",
                column: "CreateDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_CreatorId",
                table: "Setting",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_Description",
                table: "Setting",
                column: "Description");

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
                name: "PermissionValue");

            migrationBuilder.DropTable(
                name: "Setting");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "PermissionGroup");
        }
    }
}

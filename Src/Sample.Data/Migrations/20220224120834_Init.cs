using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sample.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "object_log",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    datetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    actor_id = table.Column<Guid>(type: "uuid", nullable: true),
                    object_type = table.Column<string>(type: "text", nullable: false),
                    object_id = table.Column<Guid>(type: "uuid", nullable: false),
                    before_data = table.Column<string>(type: "text", nullable: true),
                    after_data = table.Column<string>(type: "text", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_object_log", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permission",
                columns: table => new
                {
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    category_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permission", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "permission_account_group",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    create_datetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_datetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    updater_id = table.Column<Guid>(type: "uuid", nullable: true),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permission_account_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "setting",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    value = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    create_datetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_datetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    updater_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_setting", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    avatar_link = table.Column<string>(type: "text", nullable: true),
                    access_level = table.Column<int>(type: "integer", nullable: false),
                    permission_group_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    create_datetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_datetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    updater_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account", x => x.id);
                    table.ForeignKey(
                        name: "fk_account_permission_account_group_permission_group_id",
                        column: x => x.permission_group_id,
                        principalTable: "permission_account_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "permission_account_group_value",
                columns: table => new
                {
                    permission_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    permission_group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    value = table.Column<int>(type: "integer", nullable: false),
                    actual_value = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permission_account_group_value", x => new { x.permission_code, x.permission_group_id });
                    table.ForeignKey(
                        name: "fk_permission_account_group_value_permission_account_group_per~",
                        column: x => x.permission_group_id,
                        principalTable: "permission_account_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_permission_account_group_value_permission_permission_code",
                        column: x => x.permission_code,
                        principalTable: "permission",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    hash_password_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    hashed_password = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    create_datetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_datetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    updater_id = table.Column<Guid>(type: "uuid", nullable: true),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account_auth", x => x.id);
                    table.ForeignKey(
                        name: "fk_account_auth_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_session",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    expiry_datetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    create_datetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_datetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    updater_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account_session", x => x.id);
                    table.ForeignKey(
                        name: "fk_account_session_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "account",
                columns: new[] { "id", "access_level", "avatar_link", "code", "create_datetime", "creator_id", "is_deleted", "name", "permission_group_id", "update_datetime", "updater_id" },
                values: new object[] { new Guid("653dc4d4-ca05-45ac-83cd-e98fa91b890f"), 2, null, "root", null, null, true, "Root", null, null, null });

            migrationBuilder.InsertData(
                table: "permission",
                columns: new[] { "code", "category_name", "name" },
                values: new object[,]
                {
                    { "setting.export", "thiết lập", "xuất dữ liệu thiết lập" },
                    { "setting.changelog", "thiết lập", "lịch sử thay đổi của thiết lập" },
                    { "setting.update", "thiết lập", "sửa thiết lập" },
                    { "setting.read", "thiết lập", "xem thiết lập" },
                    { "permission.export", "quyền", "xuất dữ liệu quyền" },
                    { "permission.changelog", "quyền", "lịch sử thay đổi của quyền" },
                    { "permission.delete", "quyền", "xóa quyền" },
                    { "permission.update", "quyền", "sửa quyền" },
                    { "permission.read", "quyền", "xem quyền" },
                    { "permission.create", "quyền", "tạo quyền" },
                    { "account.export", "tài khoản", "xuất dữ liệu tài khoản" },
                    { "account.changelog", "tài khoản", "lịch sử thay đổi của tài khoản" },
                    { "account.delete", "tài khoản", "xóa tài khoản" },
                    { "account.update", "tài khoản", "sửa tài khoản" },
                    { "account.read", "tài khoản", "xem tài khoản" },
                    { "account.create", "tài khoản", "tạo tài khoản" },
                    { "profile.update", "thông tin cá nhân", "sửa thông tin cá nhân" }
                });

            migrationBuilder.InsertData(
                table: "permission_account_group",
                columns: new[] { "id", "code", "create_datetime", "creator_id", "is_deleted", "name", "update_datetime", "updater_id" },
                values: new object[] { new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), "admin", null, null, false, "nhóm quyền quản trị", null, null });

            migrationBuilder.InsertData(
                table: "setting",
                columns: new[] { "id", "code", "create_datetime", "creator_id", "description", "is_deleted", "update_datetime", "updater_id", "value" },
                values: new object[] { new Guid("26e84c09-8aa7-4e90-b1f6-f6e2ff6ef14c"), "code", null, null, "description", false, null, null, "value" });

            migrationBuilder.InsertData(
                table: "account",
                columns: new[] { "id", "access_level", "avatar_link", "code", "create_datetime", "creator_id", "is_deleted", "name", "permission_group_id", "update_datetime", "updater_id" },
                values: new object[] { new Guid("b81d0c90-3b91-44d4-bb00-95a5925fa5c6"), 1, null, "admin", null, null, false, "Admin", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), null, null });

            migrationBuilder.InsertData(
                table: "account_auth",
                columns: new[] { "id", "account_id", "code", "create_datetime", "creator_id", "hash_password_key", "hashed_password", "is_deleted", "type", "update_datetime", "updater_id", "username" },
                values: new object[] { new Guid("653dc4d4-ca05-45ac-83cd-e98fa91b890f"), new Guid("653dc4d4-ca05-45ac-83cd-e98fa91b890f"), null, null, null, "8sBXJjPl1BaK1ppd0PNMB366NHhmAx", "F0DC6BFBD368CDD9D63FC264C8B76E9F", false, 0, null, null, "root" });

            migrationBuilder.InsertData(
                table: "permission_account_group_value",
                columns: new[] { "permission_code", "permission_group_id", "actual_value", "value" },
                values: new object[,]
                {
                    { "setting.update", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "setting.read", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "permission.export", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "permission.changelog", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "permission.delete", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "permission.update", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "permission.read", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "permission.create", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "account.export", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "account.changelog", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "account.delete", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "account.update", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "account.read", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "account.create", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "profile.update", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "setting.changelog", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 },
                    { "setting.export", new Guid("41097c99-a6c7-4056-9ef5-be1de1fdfe77"), true, 1 }
                });

            migrationBuilder.InsertData(
                table: "account_auth",
                columns: new[] { "id", "account_id", "code", "create_datetime", "creator_id", "hash_password_key", "hashed_password", "is_deleted", "type", "update_datetime", "updater_id", "username" },
                values: new object[] { new Guid("b81d0c90-3b91-44d4-bb00-95a5925fa5c6"), new Guid("b81d0c90-3b91-44d4-bb00-95a5925fa5c6"), null, null, null, "xE8czZlAixQOJDQ0oR7PqlYJUcywj6", "73906ACDE3F2288B37120E99886A57D7", false, 0, null, null, "admin" });

            migrationBuilder.CreateIndex(
                name: "ix_account_access_level",
                table: "account",
                column: "access_level");

            migrationBuilder.CreateIndex(
                name: "ix_account_code",
                table: "account",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_account_create_datetime",
                table: "account",
                column: "create_datetime");

            migrationBuilder.CreateIndex(
                name: "ix_account_creator_id",
                table: "account",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_account_is_deleted",
                table: "account",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_account_name",
                table: "account",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_account_permission_group_id",
                table: "account",
                column: "permission_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_account_update_datetime",
                table: "account",
                column: "update_datetime");

            migrationBuilder.CreateIndex(
                name: "ix_account_updater_id",
                table: "account",
                column: "updater_id");

            migrationBuilder.CreateIndex(
                name: "ix_account_auth_account_id",
                table: "account_auth",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "ix_account_auth_code",
                table: "account_auth",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_account_auth_create_datetime",
                table: "account_auth",
                column: "create_datetime");

            migrationBuilder.CreateIndex(
                name: "ix_account_auth_creator_id",
                table: "account_auth",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_account_auth_is_deleted",
                table: "account_auth",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_account_auth_update_datetime",
                table: "account_auth",
                column: "update_datetime");

            migrationBuilder.CreateIndex(
                name: "ix_account_auth_updater_id",
                table: "account_auth",
                column: "updater_id");

            migrationBuilder.CreateIndex(
                name: "ix_account_auth_username",
                table: "account_auth",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_account_session_account_id",
                table: "account_session",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "ix_account_session_code",
                table: "account_session",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_account_session_create_datetime",
                table: "account_session",
                column: "create_datetime");

            migrationBuilder.CreateIndex(
                name: "ix_account_session_creator_id",
                table: "account_session",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_account_session_expiry_datetime",
                table: "account_session",
                column: "expiry_datetime");

            migrationBuilder.CreateIndex(
                name: "ix_account_session_is_deleted",
                table: "account_session",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_account_session_update_datetime",
                table: "account_session",
                column: "update_datetime");

            migrationBuilder.CreateIndex(
                name: "ix_account_session_updater_id",
                table: "account_session",
                column: "updater_id");

            migrationBuilder.CreateIndex(
                name: "ix_permission_category_name",
                table: "permission",
                column: "category_name");

            migrationBuilder.CreateIndex(
                name: "ix_permission_name",
                table: "permission",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_permission_account_group_code",
                table: "permission_account_group",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_permission_account_group_create_datetime",
                table: "permission_account_group",
                column: "create_datetime");

            migrationBuilder.CreateIndex(
                name: "ix_permission_account_group_creator_id",
                table: "permission_account_group",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_permission_account_group_is_deleted",
                table: "permission_account_group",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_permission_account_group_name",
                table: "permission_account_group",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_permission_account_group_update_datetime",
                table: "permission_account_group",
                column: "update_datetime");

            migrationBuilder.CreateIndex(
                name: "ix_permission_account_group_updater_id",
                table: "permission_account_group",
                column: "updater_id");

            migrationBuilder.CreateIndex(
                name: "ix_permission_account_group_value_permission_group_id",
                table: "permission_account_group_value",
                column: "permission_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_setting_code",
                table: "setting",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_setting_create_datetime",
                table: "setting",
                column: "create_datetime");

            migrationBuilder.CreateIndex(
                name: "ix_setting_creator_id",
                table: "setting",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_setting_description",
                table: "setting",
                column: "description");

            migrationBuilder.CreateIndex(
                name: "ix_setting_is_deleted",
                table: "setting",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_setting_update_datetime",
                table: "setting",
                column: "update_datetime");

            migrationBuilder.CreateIndex(
                name: "ix_setting_updater_id",
                table: "setting",
                column: "updater_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_auth");

            migrationBuilder.DropTable(
                name: "account_session");

            migrationBuilder.DropTable(
                name: "object_log");

            migrationBuilder.DropTable(
                name: "permission_account_group_value");

            migrationBuilder.DropTable(
                name: "setting");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "permission");

            migrationBuilder.DropTable(
                name: "permission_account_group");
        }
    }
}

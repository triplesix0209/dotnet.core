using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sample.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "setting",
                columns: new[] { "id", "code", "create_datetime", "creator_id", "description", "is_deleted", "update_datetime", "updater_id", "value" },
                values: new object[] { new Guid("26e84c09-8aa7-4e90-b1f6-f6e2ff6ef14c"), "code", null, null, "description", false, null, null, "value" });

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
                name: "setting");
        }
    }
}

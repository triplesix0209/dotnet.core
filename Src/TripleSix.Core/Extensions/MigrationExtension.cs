using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace TripleSix.Core.Extensions
{
    public static class MigrationExtension
    {
        public static OperationBuilder<SqlOperation> DropForeignKeyEx(this MigrationBuilder migrationBuilder, string name, string table, string schema = null)
        {
            return migrationBuilder.Sql($"ALTER TABLE {(schema.IsNotNullOrWhiteSpace() ? string.Empty : $"`{schema}.`")}`{table}` DROP FOREIGN KEY `{name}`");
        }
    }
}

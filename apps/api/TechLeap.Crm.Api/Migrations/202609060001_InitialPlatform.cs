using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TechLeap.Crm.BuildingBlocks.Persistence;

#nullable disable

namespace TechLeap.Crm.Api.Migrations;

[Migration("202609060001_InitialPlatform")]
[DbContext(typeof(PlatformDbContext))]
public partial class InitialPlatform : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "outbox_messages",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                occurred_on_utc = table.Column<DateTime>(type: "timestamptz", nullable: false),
                type = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                payload = table.Column<string>(type: "jsonb", nullable: false),
                correlation_id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                processed_on_utc = table.Column<DateTime>(type: "timestamptz", nullable: true),
                attempts = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                last_error = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                created_at_utc = table.Column<DateTime>(type: "timestamptz", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_outbox_messages", x => x.id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_outbox_messages_correlation_id",
            table: "outbox_messages",
            column: "correlation_id");

        migrationBuilder.CreateIndex(
            name: "IX_outbox_messages_processed_on_utc_created_at_utc",
            table: "outbox_messages",
            columns: new[] { "processed_on_utc", "created_at_utc" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "outbox_messages");
    }
}

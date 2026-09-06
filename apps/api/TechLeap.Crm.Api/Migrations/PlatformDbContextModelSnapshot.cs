using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TechLeap.Crm.BuildingBlocks.Persistence;

#nullable disable

namespace TechLeap.Crm.Api.Migrations;

[DbContext(typeof(PlatformDbContext))]
partial class PlatformDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "10.0.0")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        modelBuilder.Entity("TechLeap.Crm.BuildingBlocks.Persistence.OutboxMessage", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnName("id");

            b.Property<int>("Attempts")
                .ValueGeneratedOnAdd()
                .HasColumnName("attempts")
                .HasDefaultValue(0);

            b.Property<string>("CorrelationId")
                .HasMaxLength(128)
                .HasColumnName("correlation_id");

            b.Property<DateTime>("CreatedAtUtc")
                .HasColumnType("timestamptz")
                .HasColumnName("created_at_utc");

            b.Property<string>("LastError")
                .HasMaxLength(4000)
                .HasColumnName("last_error");

            b.Property<DateTime>("OccurredOnUtc")
                .HasColumnType("timestamptz")
                .HasColumnName("occurred_on_utc");

            b.Property<string>("Payload")
                .IsRequired()
                .HasColumnType("jsonb")
                .HasColumnName("payload");

            b.Property<DateTime?>("ProcessedOnUtc")
                .HasColumnType("timestamptz")
                .HasColumnName("processed_on_utc");

            b.Property<string>("Type")
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnName("type");

            b.HasKey("Id");
            b.HasIndex("CorrelationId");
            b.HasIndex("ProcessedOnUtc", "CreatedAtUtc");
            b.ToTable("outbox_messages");
        });
    }
}

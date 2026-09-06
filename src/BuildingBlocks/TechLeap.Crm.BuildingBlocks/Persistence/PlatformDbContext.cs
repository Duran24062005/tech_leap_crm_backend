using Microsoft.EntityFrameworkCore;

namespace TechLeap.Crm.BuildingBlocks.Persistence;

public sealed class PlatformDbContext(DbContextOptions<PlatformDbContext> options) : DbContext(options)
{
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var outbox = modelBuilder.Entity<OutboxMessage>();

        outbox.ToTable("outbox_messages");
        outbox.HasKey(message => message.Id);
        outbox.Property(message => message.Id).HasColumnName("id");
        outbox.Property(message => message.OccurredOnUtc).HasColumnName("occurred_on_utc").HasColumnType("timestamptz");
        outbox.Property(message => message.Type).HasColumnName("type").HasMaxLength(256).IsRequired();
        outbox.Property(message => message.Payload).HasColumnName("payload").HasColumnType("jsonb").IsRequired();
        outbox.Property(message => message.CorrelationId).HasColumnName("correlation_id").HasMaxLength(128);
        outbox.Property(message => message.ProcessedOnUtc).HasColumnName("processed_on_utc").HasColumnType("timestamptz");
        outbox.Property(message => message.Attempts).HasColumnName("attempts").HasDefaultValue(0);
        outbox.Property(message => message.LastError).HasColumnName("last_error").HasMaxLength(4000);
        outbox.Property(message => message.CreatedAtUtc).HasColumnName("created_at_utc").HasColumnType("timestamptz");
        outbox.HasIndex(message => new { message.ProcessedOnUtc, message.CreatedAtUtc });
        outbox.HasIndex(message => message.CorrelationId);
    }
}

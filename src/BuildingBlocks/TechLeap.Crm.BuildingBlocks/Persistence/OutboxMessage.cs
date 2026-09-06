namespace TechLeap.Crm.BuildingBlocks.Persistence;

public sealed class OutboxMessage
{
    public Guid Id { get; set; }

    public DateTime OccurredOnUtc { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Payload { get; set; } = string.Empty;

    public string? CorrelationId { get; set; }

    public DateTime? ProcessedOnUtc { get; set; }

    public int Attempts { get; set; }

    public string? LastError { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}

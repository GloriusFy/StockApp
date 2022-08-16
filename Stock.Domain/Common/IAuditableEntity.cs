namespace Stock.Domain.Common;

public interface IAuditableEntity
{
    string CreatedBy { get; }
    DateTime CreatedAt { get; }
    string? LastModifiedBy { get; }
    DateTime? LastModifiedAt { get; }
}
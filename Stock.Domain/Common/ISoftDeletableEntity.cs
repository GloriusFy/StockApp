namespace Stock.Domain.Common;

public interface ISoftDeletableEntity
{
    string? DeletedBy { get; }
    DateTime? DeletedAt { get; }
}
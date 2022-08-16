namespace Stock.Domain.Common;

#pragma warning disable CS8618
public class BaseEntity : IBaseEntity, ISoftDeletableEntity, IAuditableEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? DeletedBy { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public string CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string? LastModifiedBy { get; private set; }
    public DateTime? LastModifiedAt { get; private set; }
}
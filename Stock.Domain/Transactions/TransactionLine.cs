using System.ComponentModel.DataAnnotations;
using Stock.Domain.Common;
using Stock.Domain.Common.ValueObjects.Money;
using Stock.Domain.Products;

namespace Stock.Domain.Transactions;

#pragma warning disable CS8618
public class TransactionLine : IBaseEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    internal TransactionLine()
    { }
    
    [Required]
    public string ProductId { get; init; }
    public virtual Product Product { get; init; }
    
    [Required]
    public Transaction Transaction { get; init; }
    [Range(1, int.MaxValue)]
    public int Quantity { get; init; }
    [Required]
    public Money UnitPrice { get; set; }
    
    
}
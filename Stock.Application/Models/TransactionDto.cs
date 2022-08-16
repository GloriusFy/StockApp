using Stock.Application.Common.Mapping;
using Stock.Domain.Transactions;

namespace Stock.Application.Models;

#nullable disable
public sealed record TransactionDto : IMapFrom<Transaction>
{
    public string Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public int TransactionType { get; init; }
    public string PartnerName { get; init; }

    public decimal TotalAmount { get; init; }
    public string TotalCurrencyCode { get; init; }
}
#nullable restore
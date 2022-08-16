using System.ComponentModel.DataAnnotations;
using Stock.Domain.Common;
using Stock.Domain.Common.ValueObjects.Money;
using Stock.Domain.Partners;
using Stock.Domain.Products;

namespace Stock.Domain.Transactions;

#pragma warning disable CS8618
public class Transaction : BaseEntity
{
    public TransactionType TransactionType { get; private set; }

    [Required] public Money Total { get; private set; }

    public string PartnerId { get; private set; }
    public virtual Partner Partner { get; private set; }

    public virtual IReadOnlyCollection<TransactionLine> TransactionLines
        => _transactionLines.AsReadOnly();

    private List<TransactionLine> _transactionLines = new List<TransactionLine>();

    private Transaction()
    { }

    internal Transaction(TransactionType type, Partner partner)
        => (TransactionType, Partner) = (type, partner);

    internal void AddTransactionLine(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product, nameof(product));

        if (quantity < 0)
        {
            throw new ArgumentException("Value must be equal to or greater than 1.", nameof(quantity));
        }

        var transactionLine = new TransactionLine()
        {
            Transaction = this,
            Product = product,
            Quantity = quantity,
            UnitPrice = product.Price.Copy()
        };

        product.RecordTransaction(transactionLine);
        
        _transactionLines.Add(transactionLine);

        var currency = _transactionLines
            .First()
            .UnitPrice
            .Currency;

        Total = TransactionLines.Aggregate(new Money(0, currency),
            (total, line) => total + (line.UnitPrice * line.Quantity));
    }
}
using System.ComponentModel.DataAnnotations;

namespace Stock.Domain.Common.ValueObjects.Money;

#pragma warning disable CS8618
public class Money
{
    public decimal Amount { get; init; }

    [Required] public Currency Currency { get; init; }

    private Money()
    {
    }

    public Money(decimal amount, Currency? currency)
    {
        if (amount < 0) throw new ArgumentException("Value cannot be negative.", nameof(amount));
        if (currency == null) throw new ArgumentNullException(nameof(currency));

        (Amount, Currency) = (amount, currency);
    }

    public Money Copy() 
        => new Money(Amount, Currency);

    public Money AddAmount(decimal amount) 
        => new Money()
        {
            Amount = Amount + amount,
            Currency = Currency
        };

    public static Money operator +(Money left, Money right)
    {
        if (!Equals(left.Currency, right.Currency))
        {
            throw new InvalidOperationException(
                "Mixing currencies is not supported. " +
                $"Cannot add money of '{left.Currency}' and money of '{right.Currency}'.");
        }

        return left.AddAmount(right.Amount);
    }

    public static Money operator -(Money left, Money right)
    {
        if (!Equals(left.Currency, right.Currency))
        {
            throw new InvalidOperationException(
                "Mixing currencies is not supported. " +
                $"Cannot add money of '{left.Currency}' and money of '{right.Currency}'.");
        }

        return left.AddAmount(-right.Amount);
    }
    
    public static Money operator *(int scalar, Money money)
        => new Money() { Currency = money.Currency, Amount = money.Amount * scalar };

    public static Money operator *(Money money, int scalar)
        => scalar * money;

    public static Money operator *(decimal scalar, Money money)
        => new Money() { Currency = money.Currency, Amount = money.Amount * scalar };

    public static Money operator *(Money money, decimal scalar)
        => scalar * money;

    public override string ToString()
        => string.Format("{0}{1}{2:n}",
            Currency.SymbolWellKnown ? Currency.Symbol : Currency.Code,
            Currency.SymbolWellKnown ? null : " ",
            Amount);
}
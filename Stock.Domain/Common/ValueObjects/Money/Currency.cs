namespace Stock.Domain.Common.ValueObjects.Money;

#pragma warning disable CS8618
public record Currency
{
        public string Code { get; init; }
        public string Symbol { get; init; }
        public bool SymbolWellKnown { get; init; } = false;

        private static readonly IReadOnlyDictionary<string, Record> _lookup = new Dictionary<string, Record>()
        {
            ["USD"] = new Record("USD", "$", symbolWellKnown: true),
            ["EUR"] = new Record("EUR", "€", symbolWellKnown: true),
            ["RUB"] = new Record("RUB", "₽", symbolWellKnown: true),
            ["GBP"] = new Record("GBP", "£", symbolWellKnown: true)
        };

        private Currency()
        { }

        private Currency(Record record)
            => (Code, Symbol, SymbolWellKnown) = (record.Code, record.Symbol, record.SymbolWellKnown);

        /// <summary>
        /// Returns a currency instance from the provided ISO currency code string.
        /// </summary>
        /// <param name="code">Three letter ISO currency code.</param>
        /// <exception cref="ArgumentException">Thrown when the provided code is not valid or not supported by the system.</exception>
        public static Currency FromCode(string code)
        {
            if (!_lookup.TryGetValue(code.ToUpper(), out var record))
            {
                throw new ArgumentException($"Code '{code}' is not a currency code we recognize.", nameof(code));
            }

            return new Currency(record);
        }

        public static bool IsValidCurrencyCode(string code) 
            => _lookup.ContainsKey(code);

        public static List<Currency> GetAllCurrencies
            => _lookup.Select(x => new Currency(x.Value)).ToList();

        public override string ToString() 
            => Code;

        /// <summary>
        /// Returns an instance of the currency that is deemed default for the domain.
        /// </summary>
        public static Currency Default => USD;

        public static Currency USD = new Currency(_lookup["USD"]);
        public static Currency EUR = new Currency(_lookup["EUR"]);
        public static Currency RUB = new Currency(_lookup["RUB"]);
        public static Currency GBP = new Currency(_lookup["GBP"]);

        private readonly struct Record
        {
            public readonly string Code;
            public readonly string Symbol;
            public readonly bool SymbolWellKnown;

            public Record(string code, string symbol, bool symbolWellKnown = false)
                => (Code, Symbol, SymbolWellKnown) = (code, symbol, symbolWellKnown);
        }
    }
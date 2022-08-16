export interface ProductSummary {
  id: string;
  name: string;
  description: string;

  priceAmount: number;
  priceCurrencyCode: string;

  massValue: number;
  massUnitSymbol: string;

  numberInStock: number;
}

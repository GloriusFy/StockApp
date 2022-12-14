export interface Product {
  id: string;

  name: string;
  description: string;

  createdAt: Date;
  createdBy: string;

  lastModifiedAt: Date;
  lastModifiedBy: string;

  priceAmount: number;
  priceCurrencyCode: string;

  massValue: number;
  massUnitSymbol: string;

  numberInStock: number;
}

using System;

namespace ConsoleApp1
{
    partial class Program
    {
        class Product
        {
            public string ItemDescription; // opis
            public double InvoiceQuantity; // ilość opakowań
            public double InvoiceSingleQuantity;
            public double InvoiceUnitPacksize; // ilość sztuk w opakowaniu
            public double InvoiceUnitAllSingleNetPrice; // ilość sztuk * cena za sztukę
            public double InvoiceUnitNetPrice; // cena za opakowanie
            public double TaxRate; // podatek
            public double InvoiceSingleNetPrice; // cena za sztukę

            //public Product(string itemDescription, string invoiceQuantity, string invoiceUnitPacksize, string invoiceUnitNetPrice, string taxRate)
            //{
            //    ItemDescription = itemDescription;
            //    InvoiceQuantity = Convert.ToDouble(invoiceQuantity);
            //    InvoiceUnitPacksize = Convert.ToDouble(invoiceUnitPacksize);
            //    InvoiceUnitNetPrice = Convert.ToDouble(invoiceUnitNetPrice);
            //    TaxRate = Convert.ToDouble(taxRate);

            //    InvoiceSingleNetPrice = (Math.Floor((InvoiceUnitNetPrice / InvoiceUnitPacksize) * 100)) / 100;
            //    InvoiceUnitAllSingleNetPrice = InvoiceSingleNetPrice * InvoiceQuantity * InvoiceUnitPacksize;
            //}

            public Product(string itemDescription, double invoiceQuantity, double invoiceUnitPacksize, double invoiceUnitNetPrice, double taxRate)
            {
                ItemDescription = itemDescription;
                InvoiceQuantity = invoiceQuantity;
                InvoiceUnitPacksize = invoiceUnitPacksize;
                InvoiceUnitNetPrice = invoiceUnitNetPrice;
                TaxRate = taxRate;

                InvoiceSingleNetPrice = (Math.Floor((InvoiceUnitNetPrice / InvoiceUnitPacksize) * 100)) / 100;
                InvoiceUnitAllSingleNetPrice = InvoiceSingleNetPrice * InvoiceQuantity * InvoiceUnitPacksize;
            }

            public double GetInvoiceSingleNetPrice() => InvoiceSingleNetPrice;
            public double GetInvoiceUnitAllSingleNetPrice() => InvoiceUnitAllSingleNetPrice;

            public override string ToString()
            {
                return ItemDescription +
                "\n xxxx Ilosc: " + InvoiceQuantity +
                "\t cena: " + InvoiceUnitNetPrice +
                //"\t Sztuk: " + InvoiceQuantity * (InvoiceUnitPacksize) +
                "\t Cena za sztuke: " + InvoiceSingleNetPrice +
                //"\t Cena za szt * ilosc szt: " + InvoiceUnitAllSingleNetPrice +
                "\t podatek: " + TaxRate + "\n";
            }
        }
    }

}

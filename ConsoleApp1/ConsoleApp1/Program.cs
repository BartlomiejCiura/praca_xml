using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ConsoleApp1
{

    class Program
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
            public double Difference;

            public Product(string itemDescription, string invoiceQuantity, string invoiceUnitPacksize, string invoiceUnitNetPrice, string taxRate)
            {
                ItemDescription = itemDescription;
                InvoiceQuantity = Convert.ToDouble(invoiceQuantity);
                InvoiceUnitPacksize = Convert.ToDouble(invoiceUnitPacksize);
                InvoiceUnitNetPrice = Convert.ToDouble(invoiceUnitNetPrice);
                TaxRate = Convert.ToDouble(taxRate);

                InvoiceSingleNetPrice = (Math.Floor((InvoiceUnitNetPrice / InvoiceUnitPacksize) * 100)) / 100;
                InvoiceUnitAllSingleNetPrice = InvoiceSingleNetPrice * InvoiceQuantity * InvoiceUnitPacksize;
            //    Difference = Math.Round(((InvoiceQuantity * InvoiceUnitNetPrice)-InvoiceUnitAllSingleNetPrice), 2);
                // InvoiceUnitAllSingleNetPrice += Difference;
                //InvoiceSingleNetPrice += Difference / InvoiceUnitPacksize;

               // InvoiceUnitAllSingleNetPrice = InvoiceSingleNetPrice * InvoiceQuantity * InvoiceUnitPacksize;

                Console.WriteLine(Difference);
            }

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
            public double GetDiffrence() => Difference;


            public override string ToString()
            {
                return ItemDescription +
                "\n Ilosc: " + InvoiceQuantity +
                "\t cena: " + InvoiceUnitNetPrice +
                "\t Sztuk: " + InvoiceQuantity * (InvoiceUnitPacksize) +
                "\t Cena za sztuke: " + InvoiceSingleNetPrice +
                "\t Cena za szt * ilosc szt: " + InvoiceUnitAllSingleNetPrice +
                "\t podatek: " + TaxRate + "\n";
            }
        }

        class Details
        {
            private string InvoiceNumber;
            private string InvoiceDate;
            private string SalesDate;

            public Details()
            {
            }

            public Details(string invoiceNumber, string invoiceDate, string salesDate)
            {
                InvoiceNumber = invoiceNumber;
                InvoiceDate = invoiceDate;
                SalesDate = salesDate;
            }

            public override string ToString()
            {
                return "Nr zamowienia: " + InvoiceNumber + " \nData zakupu: " + SalesDate + "\nData zamowienia: " + InvoiceDate;
            }
        }

        class Seller
        {
            private string TaxID;

            public Seller(string taxID)
            {
                TaxID = taxID;
            }

            public override string ToString()
            {
                return "Nip sprzedawcy: " + TaxID + "\n";
            }
        }

        static void Main(string[] args)
        {

            List<Product> productsList = new List<Product>();

            XDocument document = LoadDocument();

            //---------------DETAILS
            var details = from d in document.Descendants("Invoice-Header")
                          select new
                          {
                              InvoiceNumber = d.Element("InvoiceNumber").Value,
                              InvoiceDate = d.Element("InvoiceDate").Value,
                              SalesDate = d.Element("SalesDate").Value,
                          };

            foreach (var d in details)
            {
                Details detailsInfo = new Details(d.InvoiceNumber, d.InvoiceDate, d.SalesDate);
                Console.WriteLine(detailsInfo.ToString());
            };

            //----------------SELLER
            var seller = from s in document.Descendants("Seller")
                         select new
                         {
                             TaxID = s.Element("TaxID").Value,
                         };
            foreach (var s in seller)
            {
                Seller sellerDetails = new Seller(s.TaxID);
                Console.WriteLine(sellerDetails.ToString());
            }

            // ------------- PRODUCTS
            var products = from r in document.Descendants("Line-Item")
                           select new
                           {
                               ItemDescription = r.Element("ItemDescription").Value,
                               InvoiceQuantity = r.Element("InvoiceQuantity").Value,
                               InvoiceUnitPacksize = r.Element("InvoiceUnitPacksize").Value,
                               InvoiceUnitNetPrice = r.Element("InvoiceUnitNetPrice").Value,
                               TaxRate = r.Element("TaxRate").Value,
                           };


            foreach (var r in products)
            {
                productsList.Add(new Product(r.ItemDescription, r.InvoiceQuantity, r.InvoiceUnitPacksize, r.InvoiceUnitNetPrice, r.TaxRate));
            }

            List<Product> wynik = new List<Product>();
            foreach (Product item in productsList)
            {
                List<Product> splitedProduct = CalculatePrice(item);
                wynik.AddRange(splitedProduct);
                Console.WriteLine(item.ToString());
            }



            double total = 0;// CalculateTotal(productsList);
            foreach (Product item in wynik)
            {
                total += item.InvoiceSingleQuantity * item.InvoiceSingleNetPrice;
            }

                Console.WriteLine("Total: " + total);

        }

        private static List<Product> CalculatePrice(Product item)
        {
            double cena = item.GetInvoiceSingleNetPrice();
            double wartoscZaokr = item.InvoiceQuantity * item.InvoiceUnitPacksize * cena;
            double wartReal = item.InvoiceQuantity * item.InvoiceUnitNetPrice;
            double r = Math.Abs( wartoscZaokr - wartReal);
            int ilosc = (int)Math.Round(r * 100); 

            Product p1 = new Product(item.ItemDescription, item.InvoiceQuantity, item.InvoiceUnitPacksize, item.InvoiceUnitNetPrice, item.TaxRate);
            p1.InvoiceSingleQuantity = ilosc;
            p1.InvoiceSingleNetPrice += 0.01;

            Product p2 = item;
            p2.InvoiceSingleQuantity = (int)(item.InvoiceQuantity * item.InvoiceUnitPacksize) - ilosc;
            List<Product> ret = new List<Product>();
            ret.Add(p1);
            ret.Add(p2);

            return ret;
        }

        private static XDocument LoadDocument()
        {
            return XDocument.Load("zamowienie.xml");
        }

        private static double CalculateTotal(List<Product> productsList)
        {
            return productsList.Sum(item => item.GetInvoiceUnitAllSingleNetPrice());
        }
    }

}

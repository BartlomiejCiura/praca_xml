namespace ConsoleApp1
{
    partial class Program
    {
        class Details
        {
            private double InvoiceNumber;
            private double InvoiceDate;
            private double SalesDate;

            public Details()
            {
            }

            public Details(double invoiceNumber, double invoiceDate, double salesDate)
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
    }

}

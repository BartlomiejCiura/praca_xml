namespace ConsoleApp1
{
    partial class Program
    {
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
    }

}

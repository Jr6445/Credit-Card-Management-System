namespace CreditCardAPI.Models
{
    public class CreditCardHolder
    {
        public int CardHolderID { get; set; }
        public string Name { get; set; }
        public string CardNumber { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal AvailableBalance => CreditLimit - CurrentBalance;
        public List<Transaction> Transactions { get; set; }
    }
}

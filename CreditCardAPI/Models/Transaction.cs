namespace CreditCardAPI.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int CardHolderID { get; set; }
        public CreditCardHolder CardHolder { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public string TransactionType { get; set; } // Compra o Pago
        public decimal Amount { get; set; }
    }
}

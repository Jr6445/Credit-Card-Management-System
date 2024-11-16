namespace CreditCardAPI.DTOs
{
    public class TransactionDTO
    {
        public int TransactionID { get; set; }
        public int CardHolderID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
    }
}

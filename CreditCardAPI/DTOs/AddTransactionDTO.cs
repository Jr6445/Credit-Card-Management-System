namespace CreditCardAPI.DTOs
{
    public class AddTransactionDTO
    {
        public int CardHolderID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public string TransactionType { get; set; } // Compra o Pago
        public decimal Amount { get; set; } // Debe ser decimal
    }
}

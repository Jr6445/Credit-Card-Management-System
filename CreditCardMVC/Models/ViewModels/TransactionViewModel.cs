namespace CreditCardMVC.Models.ViewModels
{
    public class TransactionViewModel
    {
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public string TransactionType { get; set; } // Compra o Pago
        public decimal Amount { get; set; }
    }
}

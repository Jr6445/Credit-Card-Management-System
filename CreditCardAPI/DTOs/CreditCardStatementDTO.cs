namespace CreditCardAPI.DTOs
{
    public class CreditCardStatementDTO
    {
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal TotalPurchasesThisMonth { get; set; }
        public decimal TotalPurchasesLastMonth { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal MinimumPayment { get; set; }
        public decimal TotalWithInterest { get; set; }
    }
}

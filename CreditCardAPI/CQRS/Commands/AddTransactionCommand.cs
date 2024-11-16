using MediatR;
using System;

namespace CreditCardAPI.CQRS.Commands
{
    public class AddTransactionCommand : IRequest
    {
        public int CardHolderID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
    }
}

using CreditCardAPI.DTOs;
using MediatR;
using System.Collections.Generic;

namespace CreditCardAPI.CQRS.Queries
{
    public class GetTransactionHistoryQuery : IRequest<List<TransactionDTO>>
    {
        public int CardHolderID { get; set; }
    }
}

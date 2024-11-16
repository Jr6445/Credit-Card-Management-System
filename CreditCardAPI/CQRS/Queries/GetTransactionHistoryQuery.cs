using CreditCardAPI.DTOs;
using CreditCardAPI.UnitOfWork;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CreditCardAPI.CQRS.Queries
{
    public class GetTransactionHistoryQueryHandler : IRequestHandler<GetTransactionHistoryQuery, List<TransactionDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTransactionHistoryQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TransactionDTO>> Handle(GetTransactionHistoryQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _unitOfWork.CreditCardRepository.GetTransactionsAsync(request.CardHolderID);

            return transactions.Select(t => new TransactionDTO
            {
                TransactionID = t.TransactionID,
                CardHolderID = t.CardHolderID,
                TransactionDate = t.TransactionDate,
                Description = t.Description,
                TransactionType = t.TransactionType,
                Amount = t.Amount
            }).ToList();
        }
    }
}

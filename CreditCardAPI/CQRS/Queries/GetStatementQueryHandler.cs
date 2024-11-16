using CreditCardAPI.DTOs;
using CreditCardAPI.UnitOfWork;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CreditCardAPI.CQRS.Queries
{
    public class GetStatementQueryHandler : IRequestHandler<GetStatementQuery, CreditCardStatementDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStatementQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreditCardStatementDTO> Handle(GetStatementQuery request, CancellationToken cancellationToken)
        {
            var cardHolder = await _unitOfWork.CreditCardRepository.GetCardHolderWithTransactionsAsync(request.CardHolderID);
            if (cardHolder == null) throw new Exception("Card Holder not found");

            var currentMonthTransactions = cardHolder.Transactions
                .Where(t => t.TransactionType == "Compra" &&
                            t.TransactionDate.Month == DateTime.Now.Month &&
                            t.TransactionDate.Year == DateTime.Now.Year);

            var lastMonthTransactions = cardHolder.Transactions
                .Where(t => t.TransactionType == "Compra" &&
                            t.TransactionDate.Month == DateTime.Now.AddMonths(-1).Month &&
                            t.TransactionDate.Year == DateTime.Now.Year);

            return new CreditCardStatementDTO
            {
                CardHolderName = cardHolder.Name,
                CardNumber = cardHolder.CardNumber,
                CreditLimit = cardHolder.CreditLimit,
                CurrentBalance = cardHolder.CurrentBalance,
                AvailableBalance = cardHolder.AvailableBalance,
                TotalPurchasesThisMonth = currentMonthTransactions.Sum(t => t.Amount),
                TotalPurchasesLastMonth = lastMonthTransactions.Sum(t => t.Amount),
                InterestAmount = cardHolder.CurrentBalance * 0.25m,
                MinimumPayment = cardHolder.CurrentBalance * 0.05m,
                TotalWithInterest = cardHolder.CurrentBalance + (cardHolder.CurrentBalance * 0.25m)
            };
        }
    }
}

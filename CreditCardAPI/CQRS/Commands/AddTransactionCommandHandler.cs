using CreditCardAPI.Models;
using CreditCardAPI.UnitOfWork;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CreditCardAPI.CQRS.Commands
{
    public class AddTransactionCommandHandler : IRequestHandler<AddTransactionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddTransactionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = new Transaction
            {
                CardHolderID = request.CardHolderID,
                TransactionDate = request.TransactionDate,
                Description = request.Description,
                TransactionType = request.TransactionType,
                Amount = request.Amount
            };

            // Validaciones
            if (transaction.Amount <= 0)
            {
                throw new ArgumentException("El monto debe ser mayor a 0.");
            }

            if (transaction.TransactionType != "Compra" && transaction.TransactionType != "Pago")
            {
                throw new ArgumentException("TransactionType debe ser 'Compra' o 'Pago'.");
            }

            await _unitOfWork.CreditCardRepository.AddTransactionAsync(transaction);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}

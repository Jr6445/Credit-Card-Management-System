using AutoMapper;
using CreditCardAPI.DTOs;
using CreditCardAPI.Models;
using CreditCardAPI.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreditCardAPI.Services
{
    public class CreditCardService : ICreditCardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreditCardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreditCardStatementDTO> GetStatementAsync(int cardHolderId)
        {
            var cardHolder = await _unitOfWork.CreditCardRepository.GetCardHolderWithTransactionsAsync(cardHolderId);
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

        public async Task AddTransactionAsync(AddTransactionDTO dto)
        {
            var transaction = _mapper.Map<Transaction>(dto);

            // Validar los datos antes de enviarlos al SP
            if (transaction.Amount <= 0)
            {
                throw new ArgumentException("El monto debe ser mayor a 0.");
            }

            if (transaction.TransactionType != "Compra" && transaction.TransactionType != "Pago")
            {
                throw new ArgumentException("TransactionType debe ser 'Compra' o 'Pago'.");
            }

            // Agregar la transacción usando UnitOfWork
            await _unitOfWork.CreditCardRepository.AddTransactionAsync(transaction);

            // Guardar los cambios
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<Transaction>> GetTransactionHistoryAsync(int cardHolderId)
        {
            return await _unitOfWork.CreditCardRepository.GetTransactionsAsync(cardHolderId);
        }
    }
}

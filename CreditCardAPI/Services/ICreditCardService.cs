using CreditCardAPI.DTOs;
using CreditCardAPI.Models;

namespace CreditCardAPI.Services
{
    public interface ICreditCardService
    {
        Task<CreditCardStatementDTO> GetStatementAsync(int cardHolderId);
        Task AddTransactionAsync(AddTransactionDTO dto);
        Task<List<Transaction>> GetTransactionHistoryAsync(int cardHolderId);
    }
}

using CreditCardAPI.Models;

namespace CreditCardAPI.Repositories
{
    public interface ICreditCardRepository
    {
        Task<CreditCardHolder> GetCardHolderWithTransactionsAsync(int cardHolderId);
        Task AddTransactionAsync(Transaction transaction);
        Task<List<Transaction>> GetTransactionsAsync(int cardHolderId);
    }
}

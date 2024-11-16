using CreditCardAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace CreditCardAPI.Repositories
{
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly ApplicationDbContext _context;

        public CreditCardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreditCardHolder> GetCardHolderWithTransactionsAsync(int cardHolderId)
        {
            return await _context.CreditCardHolders
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.CardHolderID == cardHolderId);
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            var sql = "EXEC AddTransaction @CardHolderID, @TransactionDate, @Description, @TransactionType, @Amount";

            await _context.Database.ExecuteSqlRawAsync(sql,
                new[]
                {
            new SqlParameter("@CardHolderID", transaction.CardHolderID),
            new SqlParameter("@TransactionDate", transaction.TransactionDate),
            new SqlParameter("@Description", transaction.Description),
            new SqlParameter("@TransactionType", transaction.TransactionType),
            new SqlParameter("@Amount", transaction.Amount)
                });
        }


        public async Task<List<Transaction>> GetTransactionsAsync(int cardHolderId)
        {
            return await _context.Transactions
                .Where(t => t.CardHolderID == cardHolderId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }
    }
}

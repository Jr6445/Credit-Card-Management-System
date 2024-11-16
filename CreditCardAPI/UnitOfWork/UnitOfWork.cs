using CreditCardAPI.Models;
using CreditCardAPI.Repositories;
using System.Threading.Tasks;

namespace CreditCardAPI.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context, ICreditCardRepository creditCardRepository)
        {
            _context = context;
            CreditCardRepository = creditCardRepository;
        }

        public ICreditCardRepository CreditCardRepository { get; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

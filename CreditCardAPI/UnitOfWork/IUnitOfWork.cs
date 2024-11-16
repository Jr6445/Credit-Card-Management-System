using CreditCardAPI.Repositories;

namespace CreditCardAPI.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICreditCardRepository CreditCardRepository { get; }
        Task<int> CompleteAsync();
        void Dispose();
    }
}

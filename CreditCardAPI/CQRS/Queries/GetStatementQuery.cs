using CreditCardAPI.DTOs;
using MediatR;

namespace CreditCardAPI.CQRS.Queries
{
    public class GetStatementQuery : IRequest<CreditCardStatementDTO>
    {
        public int CardHolderID { get; set; }
    }
}

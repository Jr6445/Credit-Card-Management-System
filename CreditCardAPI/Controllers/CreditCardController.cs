using CreditCardAPI.CQRS.Commands;
using CreditCardAPI.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CreditCardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreditCardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CreditCardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Obtener estado de cuenta
        [HttpGet("{cardHolderId}/statement")]
        public async Task<IActionResult> GetStatement(int cardHolderId)
        {
            var query = new GetStatementQuery { CardHolderID = cardHolderId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // Agregar una nueva transacción
        [HttpPost("transactions")]
        public async Task<IActionResult> AddTransaction([FromBody] AddTransactionCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Transaction added successfully." });
        }

        // Obtener historial de transacciones
        [HttpGet("{cardHolderId}/transactions")]
        public async Task<IActionResult> GetTransactionHistory(int cardHolderId)
        {
            var query = new GetTransactionHistoryQuery { CardHolderID = cardHolderId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}

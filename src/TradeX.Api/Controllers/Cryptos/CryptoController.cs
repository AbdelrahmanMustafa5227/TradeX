using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradeX.Api.Mapping;
using TradeX.Application.Cryptos.Queries.GetAll;

namespace TradeX.Api.Controllers.Cryptos
{
    [Route("api/cryptos")]
    [ApiController]
    public class CryptoController : ControllerBase
    {
        private readonly ISender _mediator;

        public CryptoController(ISender mediator)
        {
            _mediator = mediator;
        }

        //[Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateCryptoRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll([FromQuery] string? symbol, string? orderBy, int? page, int? pageSize)
        {
            var query = new GetAllCryptosQuery(symbol, orderBy, page, pageSize);
            var result = await _mediator.Send(query);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}

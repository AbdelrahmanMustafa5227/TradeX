using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradeX.Api.Mapping;

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
    }
}

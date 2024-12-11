using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradeX.Api.Mapping;

namespace TradeX.Api.Controllers.SpotOrders
{
    [Route("api/spot")]
    [ApiController]
    public class SpotOrdersController : ControllerBase
    {
        private readonly ISender _mediator;

        public SpotOrdersController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("market")]
        public async Task<IActionResult> CreateMarketOrder(CreateMarketSpotOrderRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("limit")]
        public async Task<IActionResult> CreateLimitOrder(CreateLimitSpotOrderRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelLimitOrder(CancelLimitSpotOrderRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("set-entry")]
        public async Task<IActionResult> ChangeEntryPrice(ChangeEntryPriceRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("modify")]
        public async Task<IActionResult> ModifyLimitOrder(ModifyLimitOrderRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("execute")]
        public async Task<IActionResult> ExecuteLimitOrder(ExecuteLimitOrderRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}

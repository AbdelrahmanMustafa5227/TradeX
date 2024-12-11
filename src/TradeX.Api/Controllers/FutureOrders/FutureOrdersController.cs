using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradeX.Api.Mapping;

namespace TradeX.Api.Controllers.FutureOrders
{
    [Route("api/future")]
    [ApiController]
    public class FutureOrdersController : ControllerBase
    {
        private readonly ISender _mediator;

        public FutureOrdersController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("market")]
        public async Task<IActionResult> PlaceMarketOrder(PlaceMarketFutureOrderRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("limit")]
        public async Task<IActionResult> PlaceLimitOrder(PlaceLimitFutureOrderRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("set-entry")]
        public async Task<IActionResult> ChangeEntryPrice(ChangeEntryPriceFutureRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("set-tpsl")]
        public async Task<IActionResult> SetTPSLPrice(ChangeTPSLRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("modify")]
        public async Task<IActionResult> ModifyOrder(ModifyFutureOrderRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelOrder(CancelFutureOrderRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("open")]
        public async Task<IActionResult> OpenOrder(OpenFutureOrderRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}

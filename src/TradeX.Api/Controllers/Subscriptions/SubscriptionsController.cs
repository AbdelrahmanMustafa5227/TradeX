using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradeX.Api.Mapping;

namespace TradeX.Api.Controllers.Subscriptions
{
    [Route("api/sub")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISender _mediator;

        public SubscriptionsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add-alert")]
        public async Task<IActionResult> AddAlert(AddAlertRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("remove-alert")]
        public async Task<IActionResult> RemoveAlert(RemoveAlertRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("renew")]
        public async Task<IActionResult> RenewSubscription(RenewSubscriptionRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}

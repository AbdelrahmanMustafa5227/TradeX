using MediatR;
using Microsoft.AspNetCore.Mvc;
using TradeX.Api.Mapping;
using TradeX.Application.Users.Commands.CreateUser;

namespace TradeX.Api.Controllers.Users
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISender _mediator;

        public UsersController(ISender mediatr)
        {
            _mediator = mediatr;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);

            return result.IsSuccess ?  Ok(result.Value) :  BadRequest(result.Error);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeToken(RevokeRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("setsub")]
        public async Task<IActionResult> SetSubscription(SetSubscriptionRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmKyc(Confirm_KYC_Request request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(DepositRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw(WithdrawRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(TransferRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}

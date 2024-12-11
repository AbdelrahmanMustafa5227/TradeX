using TradeX.Api.Controllers.Users;
using TradeX.Application.Users.Commands.ConfirmKYC;
using TradeX.Application.Users.Commands.CreateUser;
using TradeX.Application.Users.Commands.Deposit;
using TradeX.Application.Users.Commands.SetSubsciption;
using TradeX.Application.Users.Commands.Transfer;

namespace TradeX.Api.Mapping
{
    public static class UserMapping 
    {
        public static CreateUserCommand ToCommand(this CreateUserRequest request)
        {
            return new CreateUserCommand
            (
                 request.firstName,
                 request.lastName,
                 request.email,
                 request.password,
                 request.paymentMethod
            );
        }

        public static SetSubscriptionCommand ToCommand(this SetSubscriptionRequest request)
        {
            return new SetSubscriptionCommand
            (
                 request.UserId,
                 request.Tier,
                 request.Plan
            );
        }

        public static Confirm_KYC_Command ToCommand(this Confirm_KYC_Request request)
        {
            return new Confirm_KYC_Command
            (
                 request.UserId
            );
        }

        public static DepositCommand ToCommand(this DepositRequest request)
        {
            return new DepositCommand
            (
                 request.UserId,
                 request.DepositAmount
            );
        }

        public static WithdrawCommand ToCommand(this WithdrawRequest request)
        {
            return new WithdrawCommand
            (
                 request.UserId,
                 request.Amount
            );
        }

        public static TransferCommand ToCommand(this TransferRequest request)
        {
            return new TransferCommand
            (
                 request.SenderId,
                 request.RecepientId,
                 request.Amount
            );
        }
    }
}

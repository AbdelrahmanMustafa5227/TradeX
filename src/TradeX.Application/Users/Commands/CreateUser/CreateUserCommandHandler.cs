using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Commands.CreateUser
{
    internal class CreateUserCommandHandler : ICommandHandler<CreateUserCommand,User>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            bool isUnique = await _userRepository.IsEmailUnique(request.email);

            if (!isUnique)
                return Result.Failure<User>(UserErrors.EmailUsed);

            var user = User.Create(request.firstName , request.lastName , request.email , request.password 
                , _dateTimeProvider.UtcNow , request.paymentMethod);

            _userRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success<User>(user);
        }
    }
}

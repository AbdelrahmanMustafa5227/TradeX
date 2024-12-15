using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Users.Commands.Transfer;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Users;
using UnitTests.Domain.Users;

namespace UnitTests.Application.Users.Transfer
{
    public class TransferTests
    {
        private readonly IUserRepository _userRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public TransferTests()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        }

        [Fact]
        public async Task Transfer_SenderHasNoEnoughMoney_ShouldFail()
        {
            //arrange
            var sender = UserData.CreateKYCConfirmed();
            var receiver = UserData.CreateKYCConfirmed();
            _userRepositoryMock.GetByIdAsync(sender.Id).Returns(sender);
            _userRepositoryMock.GetByIdAsync(receiver.Id).Returns(receiver);
            //Act
            var command = new TransferCommand(sender.Id, receiver.Id, 100);
            var handler = new TransferCommandHandler(_userRepositoryMock, _unitOfWorkMock);
            var result = await handler.Handle(command, default);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.NoEnoughFunds);
        }

        [Fact]
        public async Task Transfer_RecieverNotFound_ShouldFail()
        {
            //arrange
            var sender = UserData.CreateKYCConfirmed();
            var receiver = UserData.CreateKYCConfirmed();
            _userRepositoryMock.GetByIdAsync(sender.Id).Returns(sender);
            _userRepositoryMock.GetByIdAsync(receiver.Id).Returns((User?)null);
            //Act
            var command = new TransferCommand(sender.Id, receiver.Id, 100);
            var handler = new TransferCommandHandler(_userRepositoryMock, _unitOfWorkMock);
            var result = await handler.Handle(command, default);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.UserNotFound);
        }

        [Fact]
        public async Task Transfer_SenderHasEnoughMoney_ShouldWork()
        {
            //arrange
            var sender = UserData.CreateKYCConfirmed();
            sender.Deposit(100);
            var receiver = UserData.CreateKYCConfirmed();
            _userRepositoryMock.GetByIdAsync(sender.Id).Returns(sender);
            _userRepositoryMock.GetByIdAsync(receiver.Id).Returns(receiver);
            //Act
            var command = new TransferCommand(sender.Id, receiver.Id, 100);
            var handler = new TransferCommandHandler(_userRepositoryMock, _unitOfWorkMock);
            var result = await handler.Handle(command, default);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}

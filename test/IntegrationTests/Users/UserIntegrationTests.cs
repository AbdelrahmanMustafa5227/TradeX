using Bogus;
using FluentAssertions;
using IntegrationTests.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Users.Commands.CreateUser;
using TradeX.Domain.Users;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace IntegrationTests.Users
{
    public class UserIntegrationTests : BaseIntegrationTesting
    {
        private Faker _faker;

        public UserIntegrationTests(WebAppFactory factory) : base(factory)
        {
            _faker = new Faker();
        }

        [Fact]
        public async Task CreateUser_EmailIsUsed_SHouldFail()
        {
            var email = _faker.Person.Email;
            var command = new CreateUserCommand("Abdo", "Mustafa", email , "12345" , PaymentMethod.VodafoneCash);

            var result = await Sender.Send(command);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task CreateUser_EmailIsNotUnique_SHouldFail()
        {
            var command = new CreateUserCommand("Abdo", "Mustafa", "asd@gmail.com", "12345", PaymentMethod.VodafoneCash);

            var result = await Sender.Send(command);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.EmailUsed);
        }
    }
}

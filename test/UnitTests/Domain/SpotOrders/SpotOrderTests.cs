using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Cryptos;
using TradeX.Domain.Orders;
using TradeX.Domain.Users;
using UnitTests.Domain.Cryptos;
using UnitTests.Domain.Users;

namespace UnitTests.Domain.Orders
{
    
    public class SpotOrderTests
    {
        [Fact]
        public void CancelOrder_OrderIsNotExecuted_ShouldWork()
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var crypto = CryptoData.Create();
            var order = SpotOrderData.CreateLimit(user.Id,crypto).Value;
            //Act
            var result = order.CancelOrder();
            //Assert
            result.IsSuccess.Should().BeTrue();
        }

       

    }
}

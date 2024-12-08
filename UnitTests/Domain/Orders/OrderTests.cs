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
    
    public class OrderTests
    {
        [Fact]
        public void SetOrder_UserNotKYCVerified_ShouldFail()
        {
            //Arrange
            var user = UserData.Create();
            var crypto = CryptoData.Create();
            //Act
            var result = OrderData.CreateLong(user);
            //Assert
            result.Error.Should().Be(UserErrors.KYCNotConfirmed);
        }

        [Fact]
        public void OpenOrder_OrderIsActive_ShouldFail()
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var crypto = CryptoData.Create();
            var order = OrderData.CreateLong(user).Value;
            order.OpenOrder(crypto, DateTime.UtcNow);
            //Act
            var result = order.OpenOrder(crypto, DateTime.UtcNow);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(OrderErrors.OrderAlreadyActive);
        }

        [Fact]
        public void OpenOrder_OrderIsNotActive_ShouldWork()
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var crypto = CryptoData.Create();
            var order = OrderData.CreateLong(user).Value;
            //Act
            var result = order.OpenOrder(crypto, DateTime.UtcNow);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void CloseOrder_OrderIsNotActive_ShouldFail()
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var crypto = CryptoData.Create();
            var order = OrderData.CreateLong(user).Value;
            //Act
            var result = order.CloseOrder(crypto, DateTime.UtcNow);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(OrderErrors.OrderIsNotActive);
        }

        [Fact]
        public void CloseOrder_InvalidCryptoSupplied_ShouldFail()
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var crypto = CryptoData.Create();
            var order = OrderData.CreateLong(user).Value;
            order.OpenOrder(crypto,DateTime.UtcNow);
            //Act
            var crypto2 = CryptoData.Create();
            var result = order.CloseOrder(crypto2, DateTime.UtcNow);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(OrderErrors.CryptoInvalidOrNotSet);
        }

        [Fact]
        public void CloseOrder_validCryptoSupplied_ShouldWork()
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var crypto = CryptoData.Create();
            var order = OrderData.CreateLong(user).Value;
            order.OpenOrder(crypto, DateTime.UtcNow);
            //Act
            var result = order.CloseOrder(crypto, DateTime.UtcNow);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void PlaceMarketOrder_OrderIsActive_ShouldFail()
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var crypto = CryptoData.Create();
            var order = OrderData.CreateLong(user).Value;
            order.OpenOrder(crypto, DateTime.UtcNow);
            //Act
            var result = order.PlaceMarketOrder(crypto, DateTime.UtcNow);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(OrderErrors.OrderAlreadyActive);
        }

        [Fact]
        public void PlaceMarketOrder_OrderIsNotActive_ShouldWork()
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var crypto = CryptoData.Create();
            var order = OrderData.CreateLong(user).Value;
            //Act
            var result = order.PlaceMarketOrder(crypto, DateTime.UtcNow);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void SetEntryPrice_OrderIsActive_ShouldFail()
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var crypto = CryptoData.Create();
            var order = OrderData.CreateLong(user).Value;
            order.OpenOrder(crypto,DateTime.UtcNow);
            //Act
            var result = order.SetEntryPrice(62_000);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(OrderErrors.OrderAlreadyActive);
        }

        [Fact]
        public void SetEntryPrice_OrderIsNotActive_ShouldWork()
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var order = OrderData.CreateLong(user).Value;
            //Act
            var result = order.SetEntryPrice(62_000);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void ModifyOrder_OrderIsActive_ShouldFail()
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var crypto = CryptoData.Create();
            var order = OrderData.CreateLong(user).Value;
            order.OpenOrder(crypto, DateTime.UtcNow);
            //Act
            var result = order.ModifyOrder(crypto,OrderType.Long,1.3m, 62_000);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(OrderErrors.OrderAlreadyActive);
        }

        [Fact]
        public void ModifyOrder_OrderIsNotActive_ShouldWork()
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var crypto = CryptoData.Create();
            var order = OrderData.CreateLong(user).Value;
            //Act
            var result = order.ModifyOrder(crypto, OrderType.Long, 1.3m, 62_000);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void CancelOrder_OrderIsActive_ShouldFail()
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var crypto = CryptoData.Create();
            var order = OrderData.CreateLong(user).Value;
            order.OpenOrder(crypto, DateTime.UtcNow);
            //Act
            var result = order.CancelOrder();
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(OrderErrors.OrderAlreadyActive);
        }

        [Fact]
        public void CancelOrder_OrderIsNotActive_ShouldWork()
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var crypto = CryptoData.Create();
            var order = OrderData.CreateLong(user).Value;
            //Act
            var result = order.CancelOrder();
            //Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Theory]
        [InlineData(90_500,92_000,true)]
        [InlineData(91_500, 92_000, false)]
        [InlineData(89_000, 90_000, false)]
        public void SetStopLossTakeProfit_OrderLong_InActiveOrder(decimal sl, decimal tp , bool isSuccess)
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var crypto = CryptoData.Create();
            var order = OrderData.CreateLong(user).Value;
            //Act
            var result = order.SetStopLoseTakeProfitPrice(crypto,sl,tp);
            //Assert
            Assert.Equal(isSuccess, result.IsSuccess);
        }

        [Theory]
        [InlineData(91_500, 90_000, true)]
        [InlineData(91_500, 93_000, false)]
        [InlineData(89_000, 90_000, false)]
        public void SetStopLossTakeProfit_OrderShort_InActiveOrder(decimal sl, decimal tp, bool isSuccess)
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var crypto = CryptoData.Create();
            var order = OrderData.CreateShort(user).Value;
            //Act
            var result = order.SetStopLoseTakeProfitPrice(crypto, sl, tp);
            //Assert
            Assert.Equal(isSuccess, result.IsSuccess);
        }

        [Theory]
        [InlineData(89_500, 93_000, true)]
        [InlineData(91_500, 92_000, false)]
        [InlineData(89_000, 89_500, false)]
        public void SetStopLossTakeProfit_OrderLong_ActiveOrder(decimal sl, decimal tp, bool isSuccess)
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var crypto = CryptoData.Create();
            var order = OrderData.CreateLong(user).Value;
            order.OpenOrder(crypto, DateTime.UtcNow);
            //Act
            var result = order.SetStopLoseTakeProfitPrice(crypto, sl, tp);
            //Assert
            Assert.Equal(isSuccess, result.IsSuccess);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.FutureOrders;
using UnitTests.Domain.Cryptos;
using UnitTests.Domain.Users;

namespace UnitTests.Domain.FutureOrders
{
    public class FutureOrdersTests
    {
        [Theory]
        [InlineData(90_500, 92_000, true)]
        [InlineData(91_500, 92_000, false)]
        [InlineData(89_000, 90_000, false)]
        public void SetStopLossTakeProfit_OrderLong_InactiveOrder(decimal sl, decimal tp, bool isSuccess)
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var crypto = CryptoData.Create();
            var order = FutureOrderData.CreateLimit(user.Id ,crypto).Value;
            //Act
            var result = order.SetStopLoseTakeProfitPrice(crypto, tp, sl);
            //Assert
            Assert.Equal(isSuccess, result.IsSuccess);
        }

        [Theory]
        [InlineData(91_500, 90_000, true)]
        [InlineData(91_500, 93_000, false)]
        [InlineData(89_000, 90_000, false)]
        public void SetStopLossTakeProfit_OrderShort_InactiveOrder(decimal sl, decimal tp, bool isSuccess)
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var crypto = CryptoData.Create();
            var order = FutureOrderData.CreateLimitShort(user.Id, crypto).Value;
            //Act
            var result = order.SetStopLoseTakeProfitPrice(crypto, tp, sl);
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
            var order = FutureOrderData.CreateLimit(user.Id, crypto).Value;
            order.OpenOrder(DateTime.UtcNow);
            //Act
            var result = order.SetStopLoseTakeProfitPrice(crypto, tp, sl);
            //Assert
            Assert.Equal(isSuccess, result.IsSuccess);
        }
    }
}

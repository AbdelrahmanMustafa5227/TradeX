using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Users;
using UnitTests.Domain.Cryptos;
using UnitTests.Domain.Orders;
using UnitTests.Domain.Users;

namespace UnitTests.Domain.Subscrption
{
    public class SubscriptionTests
    {
        [Fact]
        public void AddAlert_LessThanSubscriptionAllows_ShouldWork()
        {
            //Arrange
            var user = UserData.Create();
            user.ConfirmKYC();
            var alert = UserData.CreateAlert();
            var subscription = SubscriptionData.Create(user);
            user.SetSubscription(subscription);
            //Act
            var result = subscription.AddAlert(alert, DateTime.UtcNow);
            //Assert
            result.IsSuccess.Should().BeTrue();
            subscription.Alerts.Count.Should().Be(1);
        }

        [Fact]
        public void AddAlert_MoreThanSubscriptionAllows_ShouldFail()
        {
            //Arrange
            var user = UserData.Create();
            user.ConfirmKYC();
            var subscription = SubscriptionData.Create(user);
            user.SetSubscription(subscription);

            var alert1 = UserData.CreateAlert();
            var alert2 = UserData.CreateAlert();
            var alert3 = UserData.CreateAlert();
            var alert4 = UserData.CreateAlert();


            //Act
            var result1 = subscription.AddAlert(alert1, DateTime.UtcNow);
            var result2 = subscription.AddAlert(alert2, DateTime.UtcNow);
            var result3 = subscription.AddAlert(alert3, DateTime.UtcNow);
            var result4 = subscription.AddAlert(alert4, DateTime.UtcNow);
            //Assert
            result3.IsSuccess.Should().BeTrue();
            result4.IsSuccess.Should().BeFalse();
            result4.Error.Should().Be(SubscriptionErrors.ExceededAlertLimit);
        }

        [Fact]
        public void RemoveAlert_AlertIsNotFound_ShouldFail()
        {
            //Arrange
            var user = UserData.Create();
            user.ConfirmKYC();
            var subscription = SubscriptionData.Create(user);
            user.SetSubscription(subscription);
            var alert = UserData.CreateAlert();
            //Act
            var result = subscription.RemoveAlert(alert.Id);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(SubscriptionErrors.AlertNotFound);
        }

        [Fact]
        public void RemoveAlert_AlertIsFound_ShouldWork()
        {
            //Arrange
            var user = UserData.Create();
            user.ConfirmKYC();
            var subscription = SubscriptionData.Create(user);
            user.SetSubscription(subscription);
            var alert = UserData.CreateAlert();
            subscription.AddAlert(alert, DateTime.UtcNow);
            //Act
            var result = subscription.RemoveAlert(alert.Id);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}

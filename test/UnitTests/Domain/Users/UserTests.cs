using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Users;
using UnitTests.Domain.Subscrption;

namespace UnitTests.Domain.Users
{
    public class UserTests
    {
        [Fact]
        public void RequestKYC_KYCNotConfirmed_ShouldReturnSuccess()
        {
            //Arrange
            var user = UserData.Create();
            //Act
            var result = user.RequestKYCConfirmation();
            //Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void RequestKYC_KYCConfirmed_ShouldReturnError()
        {
            //Arrange
            var user = UserData.Create();
            user.ConfirmKYC();
            //Act
            var result = user.RequestKYCConfirmation();
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.KYCAlreadyConfirmed);
        }

        [Fact]
        public void ConfirmKYC_KYCNotConfirmed_ShouldReturnSuccess()
        {
            //Arrange
            var user = UserData.Create();
            //Act
            var result = user.ConfirmKYC();
            //Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void ConfirmKYC_KYCConfirmed_ShouldReturnError()
        {
            //Arrange
            var user = UserData.Create();
            user.ConfirmKYC();
            //Act
            var result = user.ConfirmKYC();
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.KYCAlreadyConfirmed);
        }

        [Fact]
        public void SetSubscription_KYCNotConfirmed_ShouldFail()
        {
            //Arrange
            var user = UserData.Create();
            var sub = SubscriptionData.Create(user);
            //Act
            var result = user.SetSubscription(sub);
            //Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void SetSubscription_KYCConfirmed_OnActiveSubscription_EnoughBalance_ShouldFail()
        {
            //Arrange
            var user = UserData.Create();
            user.ConfirmKYC();
            user.Deposit(50);
            var sub = SubscriptionData.Create(user);
            user.SetSubscription(sub);
            //Act
            var result = user.SetSubscription(sub);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.SubcriptionAlreadyActive);
        }

        [Fact]
        public void SetSubscription_KYCConfirmed_NotOnActiveSubscription_NoEnoughBalance_ShouldFail()
        {
            //Arrange
            var user = UserData.Create();
            user.ConfirmKYC();
            var sub = SubscriptionData.CreateBronze(user);
            //Act
            var result = user.SetSubscription(sub);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.NoEnoughFunds);
        }

        [Fact]
        public void SetSubscription_KYCConfirmed_NotOnActiveSubscription_EnoughBalance_ShouldWork()
        {
            //Arrange
            var user = UserData.Create();
            user.ConfirmKYC();
            user.Deposit(20);
            var sub = SubscriptionData.Create(user);
            //Act
            var result = user.SetSubscription(sub);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Deposit_KYCConfirmed_ShouldWork()
        {
            //Arrange
            var user = UserData.Create();
            user.ConfirmKYC();
            //Act
            var result = user.Deposit(20);
            //Assert
            result.IsSuccess.Should().BeTrue();
            user.balance.TotalBalance.Should().Be(20);
            user.balance.FreezedBalance.Should().Be(0);
            user.balance.AvailableBalance.Should().Be(20);
        }

        [Fact]
        public void Deposit_KYCNotConfirmed_ShouldWork()
        {
            //Arrange
            var user = UserData.Create();
            //Act
            var result = user.Deposit(20);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.KYCNotConfirmed);
        }

        [Fact]
        public void Withdraw_KYCConfirmed_NoEnoughBalance_ShouldFail()
        {
            //Arrange
            var user = UserData.Create();
            user.ConfirmKYC();
            //Act
            var result = user.Withdraw(20);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.NoEnoughFunds);
        }

        [Fact]
        public void Withdraw_KYCConfirmed_EnoughBalance_ShouldWork()
        {
            //Arrange
            var user = UserData.Create();
            user.ConfirmKYC();
            user.Deposit(20);
            //Act
            var result = user.Withdraw(20);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Transfer_SendKYCConfirmed_ReciepntKYCNotConfirmed_ShouldFail()
        {
            //Arrange
            var sender = UserData.Create();
            sender.ConfirmKYC();
            sender.Deposit(20);
            var Reciepnt = UserData.Create();
            //Act
            var result = sender.Transfer(20,Reciepnt);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.KYCNotConfirmed);
        }

        [Fact]
        public void Transfer_SendKYCConfirmed_ReciepntKYCConfirmed_NoEnoughBalance_ShouldFail()
        {
            //Arrange
            var sender = UserData.Create();
            sender.ConfirmKYC();
            sender.Deposit(20);
            var Reciepnt = UserData.Create();
            Reciepnt.ConfirmKYC();
            //Act
            var result = sender.Transfer(30, Reciepnt);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.NoEnoughFunds);
        }

        [Fact]
        public void Transfer_SendKYCConfirmed_ReciepntKYCConfirmed_EnoughBalance_ShouldWork()
        {
            //Arrange
            var sender = UserData.Create();
            sender.ConfirmKYC();
            sender.Deposit(20);
            var Reciepnt = UserData.Create();
            Reciepnt.ConfirmKYC();
            //Act
            var result = sender.Transfer(10, Reciepnt);
            //Assert
            result.IsSuccess.Should().BeTrue();
            sender.balance.AvailableBalance.Should().Be(10);
            Reciepnt.balance.AvailableBalance.Should().Be(10);
        }

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
            var result = user.AddAlert(alert,subscription);
            //Assert
            result.IsSuccess.Should().BeTrue();
            user.Alerts.Count.Should().Be(1);
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
            var result1 = user.AddAlert(alert1, subscription);
            var result2 = user.AddAlert(alert2, subscription);
            var result3 = user.AddAlert(alert3, subscription);
            var result4 = user.AddAlert(alert4, subscription);
            //Assert
            result3.IsSuccess.Should().BeTrue();
            result4.IsSuccess.Should().BeFalse();
            result4.Error.Should().Be(UserErrors.ExceededAlertLimit);
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
            var result= user.RemoveAlert(alert.Id);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.AlertNotFound);
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
            user.AddAlert(alert,subscription);
            //Act
            var result = user.RemoveAlert(alert.Id);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void AddAsset_AssetExists_ShouldAddtoCurrentValue()
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var asset = Asset.Create(Guid.NewGuid(), 10);
            user.AddAsset(asset);
            //Act
            var result = user.AddAsset(asset);
            //Assert
            result.IsSuccess.Should().BeTrue();
            user.Assets.Count.Should().Be(1);
            user.Assets[0].Amount.Should().Be(20);
        }

        [Fact]
        public void AddAsset_AssetNotExists_ShouldAddAsNew()
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var asset = Asset.Create(Guid.NewGuid(), 10);
            //Act
            var result = user.AddAsset(asset);
            //Assert
            result.IsSuccess.Should().BeTrue();
            user.Assets.Count.Should().Be(1);
            user.Assets[0].Amount.Should().Be(10);
        }

        [Fact]
        public void RemoveAsset_AssetNotExists_ShouldFail()
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var asset = Asset.Create(Guid.NewGuid(), 10);
            //Act
            var result = user.RemoveAsset(asset.Id , 10);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.AssetNotFound);
        }

        [Fact]
        public void RemoveAsset_AssetExists_NoEnoughAmount_ShouldFail()
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var asset = Asset.Create(Guid.NewGuid(), 10);
            user.AddAsset(asset);
            //Act
            var result = user.RemoveAsset(asset.Id, 20);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.NoEnoughFunds);
        }

        [Fact]
        public void RemoveAsset_AssetExists_EnoughAmount_ShouldWork()
        {
            //Arrange
            var user = UserData.CreateKYCConfirmed();
            var asset = Asset.Create(Guid.NewGuid(), 10);
            user.AddAsset(asset);
            //Act
            var result = user.RemoveAsset(asset.Id, 10);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}

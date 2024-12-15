using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Users;
using UnitTests.Domain.Subscrption;

namespace UnitTests.Domain.Users
{
    public class UserData
    {

        public static readonly string firstName = "Abdo";
        public static readonly string lastName = "Mustafa";
        public static readonly string email = "test@test.com";
        public static readonly string password = "123456";
        public static readonly DateTime createdOn = DateTime.UtcNow;
        public static readonly PaymentMethod paymentMethod = PaymentMethod.Fawry;

        public static User Create()
        {
            return User.Create(firstName, lastName, email, password, createdOn, paymentMethod);
        }

        public static User CreateKYCConfirmed()
        {
           var user = User.Create(firstName, lastName, email, password, createdOn, paymentMethod);
            user.ConfirmKYC();
            return user;
        }

        public static User CreateWithBasicSubscription()
        {
            var user = User.Create(firstName, lastName, email, password, createdOn, paymentMethod);
            user.ConfirmKYC();
            var sub = SubscriptionData.Create(user);
            user.SetSubscription(sub);
            return user;
        }

        public static Alert CreateAlert()
        {
            return Alert.Create(Guid.NewGuid(), 100);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Orders;

namespace TradeX.Domain.Users
{
    public interface IUserRepository
    {
        void Add (User user);

        Task<User?> GetByIdAsync (Guid id);

        Task<bool> IsEmailUnique(string email);

    }
}

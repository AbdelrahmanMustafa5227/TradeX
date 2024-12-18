using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.Abstractions
{
    public interface IPasswordHasher
    {
        public string Hash(string password);
        bool Verify(string password, string passwordHash);
    }
}

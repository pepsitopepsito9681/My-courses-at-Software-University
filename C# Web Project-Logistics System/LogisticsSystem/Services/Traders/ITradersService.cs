using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticsSystem.Services.Traders
{
    public interface ITradersService
    {
        void Create(
            string userId,
            string name,
            string telephoneNumber
            );

        public bool IsUserTrader(string userId);

        string IdByUser(string userId);

        string TelephoneNumberByUser(string userId);
    }
}

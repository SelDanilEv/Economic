using EconoMiC.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconoMiC.Logic
{
    public class MoneyController
    {
        public MoneyController()
        {

        }

        public bool AddMoney(User user, double money)
        {
            try
            {
                user.Money = Math.Round(user.Money + money, 2, MidpointRounding.AwayFromZero);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TakeMoney(User user, double money)
        {
            try
            {
                if (user.Money - money < 0)
                {
                    return false;
                }
                user.Money = Math.Round(user.Money - money, 2, MidpointRounding.AwayFromZero);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}

using EconoMiC.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconoMiC.Data
{
    static class TempData
    {
        static List<User> accs = new List<User>
        {
            new User("Danil","12345"),
            new User("Misha","12345"),
            new User("Max","Max"),
        };

        public static bool validate(string log, string pass)   // check login and password and set last activity date
        {
            if (accs.Exists(x => x.Login == log))
            {
                var temp = accs.First(x => x.Login == log);
                if (temp.Password == pass)
                {
                    temp.lastVisitDate = new Calendar.SomeDate(System.DateTime.Now.Day, System.DateTime.Now.Month, DateTime.Now.Year);
                    return true;
                }
            }
            return false;
        }

        static public User GetUser(string login)      
        {
            return accs.First(x => x.Login == login);
        }

        public static bool addUser(User user)
        {
            if (!accs.Exists(x => x.Login == user.Login))
            {
                accs.Add(user);
                return true;
            }
            return false;
        }
    }
}

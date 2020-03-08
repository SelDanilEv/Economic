using EconoMiC.Calendar;
using EconoMiC.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconoMiC.Users
{
    public class User
    {
        private string _login;
        private double _money;
        private string _password;
        public SomeDate lastVisitDate;

        public string Password { get => _password; }
        public string Login { get => _login; }
        public double Money { get => _money; set => _money = value; }

        public List<Target> targets = new List<Target>();
        public List<Category> categories = new List<Category>();
        public List<Income> incomes = new List<Income>();

        public User(string login, string password)
        {
            this._login = login;
            this._password = password;
        }

        public bool AddTarget(Target target)
        {
            if (!targets.Exists(x => x.TargetName == target.TargetName))
                targets.Add(target);
            return false;
        }
    }
}

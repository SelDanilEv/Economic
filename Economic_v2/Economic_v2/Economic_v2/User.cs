using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2
{
    class User
    {
        public User(int iD, string login, string password, double total_money, DateTime last_activity)
        {
            ID = iD;
            Login = login;
            Password = password;
            Total_money = total_money;
            Last_activity = last_activity;
        }

        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        [Column(TypeName = "float")]
        public double Total_money { get; set; }
        [Column(TypeName = "date")]
        public DateTime Last_activity { get; set; }

        public virtual List<Target> Targets { get; set; }
        public virtual List<Category> Categories { get; set; }
        public virtual List<Income> Incomes{ get; set; }
    }
}

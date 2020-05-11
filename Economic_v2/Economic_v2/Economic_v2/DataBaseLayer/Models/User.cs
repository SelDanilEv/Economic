using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public double Total_money { get; set; }
        public double Free_money { get; set; }
        [Column(TypeName = "date")]
        public DateTime Last_activity { get; set; }

        public List<ActiveTarget> ActiveTargets { get; set; }
        public List<OldTarget> OldTargets  { get; set; }
        public List<SuspendedTarget> SuspendedTargets { get; set; }
        public List<Category> Categories { get; set; }
        public List<Income> Incomes{ get; set; }
        public List<OneTimeTransaction> OneTimeTransactions { get; set; }
        public List<AdjustmentContract> AdjustmentContracts { get; set; }
        public Node Node { get; set; }

        #region constructors
        public User()
        {
        }

        public User(string login, string password, double total_money, DateTime last_activity)
        {
            Login = login;
            Password = password;
            Total_money = total_money;
            Last_activity = last_activity;
        }
        #endregion constructors

        public void UpdateLastActivity(DateTime last_activity)
        {
            this.Last_activity = last_activity;
        }
    }
}

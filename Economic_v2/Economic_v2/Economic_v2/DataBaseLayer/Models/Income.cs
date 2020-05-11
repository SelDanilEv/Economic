using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.Models
{
    public class Income
    {
        public int Id { get; set; }
        
        public string IncomeName { get; set; }
        public double Money { get; set; }
        public DateTime Date { get; set; }

        #region constructors
        public Income()
        {
        }

        public Income(string incomeName, double money, DateTime date)
        {
            IncomeName = incomeName;
            Money = money;
            Date = date;
        }
        #endregion constructors
    }
}

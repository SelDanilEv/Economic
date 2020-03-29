using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2
{
    class Income
    {
        public int ID { get; set; }

        public string IncomeName { get; set; }
        public double Money { get; set; }
        public DateTime Date { get; set; }

        public Income(string incomeName, double money,DateTime date)
        {
            IncomeName = incomeName;
            Money = money;
            Date = date;
        }

    }
}

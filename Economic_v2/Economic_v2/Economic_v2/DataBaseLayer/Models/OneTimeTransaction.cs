using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.Models
{
    public class OneTimeTransaction
    {
        public int Id { get; set; }
        
        public string TransactionName { get; set; }
        public double Spend { get; set; }
        public DateTime Date { get; set; }

        #region constructors
        public OneTimeTransaction()
        {
        }

        public OneTimeTransaction(string transactionName, double spend, DateTime date)
        {
            TransactionName = transactionName;
            Spend = spend;
            Date = date;
        }
        #endregion constructors
    }
}

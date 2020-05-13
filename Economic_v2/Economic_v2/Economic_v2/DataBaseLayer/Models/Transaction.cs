using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        
        public string TransactionName { get; set; }
        public double Spend { get; set; }
        public DateTime Date { get; set; }

        [NotMapped]
        public string ShortDate
        {
            get => Date.ToShortDateString();
        }
        #region constructors
        public Transaction()
        {
        }

        public Transaction(Transaction transaction):
            this(transaction.TransactionName,transaction.Spend,transaction.Date)
        {
            this.Id = transaction.Id;
        }

        public Transaction(string transactionName, double spend, DateTime date)
        {
            TransactionName = transactionName;
            Spend = spend;
            Date = date;
        }
        #endregion constructors
    }
}

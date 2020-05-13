using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        [NotMapped]
        public int GetDayOfIncome
        {
            get => Date.Day;
        }

        #region constructors
        public Income()
        {
        }

        public Income(Income income):
            this(income.IncomeName,income.Money,income.Date)
        {
            this.Id = income.Id;
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

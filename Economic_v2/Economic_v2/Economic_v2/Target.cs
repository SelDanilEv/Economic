using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2
{
    class Target
    {
        public int ID { get; set; }

        public string TargetName { get; set; }
        [Column(TypeName = "float")]
        public double Spend { get; set; }
        [Column(TypeName = "float")]
        public double TotalSum { get; set; }
        [Column(TypeName = "float")]
        public double CurrentSum { get; set; }
        [Column(TypeName = "date")]
        public DateTime TargetTime { get; set; }

        public Target(string name, double totalSum, double curentSum)
        {
            TargetName = name;
            TotalSum = Math.Round(totalSum, 2, MidpointRounding.AwayFromZero);
            CurrentSum = Math.Round(curentSum, 2, MidpointRounding.AwayFromZero);
        }

        public Target(string name, double totalSum, double curentSum,double spend):this(name,totalSum,curentSum)
        {
            Spend = Math.Round(spend, 2, MidpointRounding.AwayFromZero);
            TargetTime = DateTime.Today;
            // add time
        }

        public Target(string name, double totalSum, double curentSum,DateTime targ_time):this(name,totalSum,curentSum)
        {
            TargetTime = targ_time;
            // add spend
        }
    }
}

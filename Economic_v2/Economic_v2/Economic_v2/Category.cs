using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2
{
    class Category
    {
        public int ID { get; set; }

        public string CategoryName { get; set; }
        public double Spend { get; set; }
    
        public Category(string name, double spend)
        {
            CategoryName = name;
            Spend = Math.Round(spend, 2, MidpointRounding.AwayFromZero);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.Models
{
    public class Adjustment
    {
        public int Id { get; set; }

        public double OldVal { get; set; }
        public double NewVal { get; set; }
        public string Description { get; set; }

        #region constructors
        public Adjustment()
        {
        }

        public Adjustment(int id, double oldVal, double newVal, string description)
        {
            Id = id;
            OldVal = oldVal;
            NewVal = newVal;
            Description = description;
        }
        #endregion constructors
    }
}

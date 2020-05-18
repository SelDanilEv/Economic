using Economic_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.Models
{
    public class Statistic
    {
        public int Id { get; set; }

        public int CounterTargets { get; set; }
        public int MoodChange { get; set; }
        public Target LargestTarget { get; set; }


        public Statistic()
        {
            MoodChange = 0;
            CounterTargets = 0;
        }
    }
}

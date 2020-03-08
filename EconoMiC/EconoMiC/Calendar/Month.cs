using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconoMiC.Calendar
{
    public enum month
    {
        Jan =0,
        Feb,
        Mar,
        Apr,
        May,
        Jun,
        Jul,
        Aug,
        Sep,
        Oct,
        Nov,
        Dec
    }

    public class Month
    {
        public List<Day> days = new List<Day>();
        public month Mounth;

        public Month(int numberMonth,int numberOfDay)
        {
            Mounth = (month)numberMonth;
            for (int i = 1; i <= numberMonth; i++)
                days.Add(new Day(i));
        }
    }
}

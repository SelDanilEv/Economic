using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconoMiC.Calendar
{
    class Year
    {
        public int number;

        public List<Month> months = new List<Month>();

        public Year(int numberOfYear)
        {
            months.Add(new Month(1, 31));
            if (numberOfYear % 4 == 0)
                months.Add(new Month(2, 29));
            else
                months.Add(new Month(2, 28));
            months.Add(new Month(3, 31));
            months.Add(new Month(4, 30));
            months.Add(new Month(5, 31));
            months.Add(new Month(6, 30));
            months.Add(new Month(7, 31));
            months.Add(new Month(8, 31));
            months.Add(new Month(9, 30));
            months.Add(new Month(10, 31));
            months.Add(new Month(11, 30));
            months.Add(new Month(12, 31));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconoMiC.Calendar
{
    class Calendar
    {
        public Year currentYear;
        public Year NextYear;

        public Calendar() 
        {
            int curYear = System.DateTime.Today.Year;

            currentYear = new Year(curYear);
            NextYear = new Year(++curYear);
        }
    }
}

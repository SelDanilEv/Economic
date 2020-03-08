using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconoMiC.Calendar
{
    class CalendarHandler    // singleton
    {
        static Calendar Calendar;

        private CalendarHandler() { }

        public Calendar getCalendar()
        {
            if (Calendar == null)
                return Calendar = new Calendar();
            if (Calendar.currentYear.number ==System.DateTime.Now.Year)
                return Calendar = new Calendar();
            return Calendar;
        }


    }
}

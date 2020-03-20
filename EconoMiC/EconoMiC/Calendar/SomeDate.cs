using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconoMiC.Calendar
{
   public class SomeDate
    {
        public int day;
        public month month;
        public int year;

        public SomeDate()
        {
        }

        public SomeDate(int day, int month, int year)
        {
            this.day = day;
            this.month = (month)month;
            this.year = year;
        }

        public void FromDateToSomeDate(DateTime dateTime)
        {
            day = dateTime.Day;
            month = (month)dateTime.Month;
            year = dateTime.Year;
        }
    }
}

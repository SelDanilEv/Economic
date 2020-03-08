using EconoMiC.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconoMiC.Elements
{
    public class Income
    {
        public string name;
        public int day;
        public month month;
        public double money;

        public Income(string name,double money, int day, int month)
        {
            this.name = name;
            this.day = day;
            this.month = (month)month;
            this.money = money;
        }
    }
}

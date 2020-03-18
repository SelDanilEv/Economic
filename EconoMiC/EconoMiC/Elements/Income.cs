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
        private string _incomeName;
        private int _day;
        private month _month;
        private double _money;

        public Income(string incomeName, int day, month month, double money)
        {
            _incomeName = incomeName;
            _day = day;
            _month = month;
            _money = money;
        }

        public string IncomeName { get => _incomeName; set => _incomeName = value; }
        public int Day { get => _day; set => _day = value; }
        public month Month { get => _month; set => _month = value; }
        public double Money { get => _money; set => _money = value; }
    }
}

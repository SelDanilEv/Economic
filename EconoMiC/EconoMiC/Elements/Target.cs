using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconoMiC.Elements
{
    public class Target
    {
        private string _targetName;
        private double _spend;
        private double _totalSum;
        private double _currentSum;

        public Target(string name,double totalSum, double curentSum, double spend)
        {
            _targetName = name;
            _spend = Math.Round(spend, 2, MidpointRounding.AwayFromZero);
            _totalSum = Math.Round(totalSum, 2, MidpointRounding.AwayFromZero);
            _currentSum = Math.Round(curentSum, 2, MidpointRounding.AwayFromZero);
        }

        public string TargetName { get => _targetName; }
        public double Spend { get => _spend; }
        public double TotalSum { get => _totalSum; set => _totalSum = value; }
        public double CurrentSum { get => _currentSum; set => _currentSum = value; }
    }
}

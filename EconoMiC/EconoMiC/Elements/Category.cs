using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconoMiC.Elements
{
    public class Category
    {
        private string _categoryName;
        private double _spend;

        public Category(string name, double spend)
        {
            _categoryName = name;
            _spend = Math.Round(spend, 2, MidpointRounding.AwayFromZero);

        }

        public string CategoryName { get => _categoryName;}
        public double Spend { get => _spend;}
    }
}

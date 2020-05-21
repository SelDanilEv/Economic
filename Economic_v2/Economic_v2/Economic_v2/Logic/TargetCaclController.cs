using Economic_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.Logic
{
    class TargetCalcController
    {
        public TargetCalcController()
        {

        }

        public ActiveTarget CalcTarget(ActiveTarget target, int month, ref double reservedSum)
        {
            double reserve = target.CurrentSum + month * target.Spend;
            if (reserve > target.TotalSum)
                reserve = target.TotalSum;
            target.CurrentSum = reserve;
            reservedSum += reserve;
            return target;
        }

        public Target CalculateTargetWithSpend(Target target)
        {
            double neededMoney = target.TotalSum - target.CurrentSum;
            DateTime targetTime = DateTime.Parse("1/" + DateTime.Now.Month.ToString() + '/' + DateTime.Now.Year.ToString());
            int month = (int)(neededMoney / target.Spend);

            if (month > 120 || month < 0)
            {
                month = 120;
            }

            if (neededMoney % target.Spend != 0)
            {
                targetTime = targetTime.AddMonths(1);
            }

            target.TargetTime = targetTime.AddMonths(month);

            return target;
        }


        private double RoundUp(double number, int digits)
        {
            var factor = Convert.ToDouble(Math.Pow(10, digits));
            return Math.Ceiling(number * factor) / factor;
        }

        public Target CalculateTargetWithTime(Target target)
        {
            double neededMoney = target.TotalSum - target.CurrentSum;
            DateTime targetTime = DateTime.Parse(
                "1/" + DateTime.Now.Month.ToString() + '/' + DateTime.Now.Year.ToString());

            int month = target.TargetTime.Month - DateTime.Today.AddMonths(1).Month +
                12 * (target.TargetTime.Year - DateTime.Today.Year);

            if (neededMoney % month == 0)
                target.Spend = RoundUp(neededMoney / month, 2);
            else
                target.Spend = RoundUp(neededMoney / month + 1, 2);

            return target;
        }
    }
}

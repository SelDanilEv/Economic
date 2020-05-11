using Economic_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.Builders
{
    public class TargetBuilder : Target
    {
        public TargetBuilder()
        {
        }

        public TargetBuilder(Target target)
        {
            if (target != null)
            {
                this.Id = target.Id;
                this.TargetName = target.TargetName;
                this.TotalSum = target.TotalSum;
                this.CurrentSum = target.CurrentSum;
                this.Spend = target.Spend;
                this.TargetTime = target.TargetTime;
            }
        }

        public TargetBuilder SetName(string name)
        {
            this.TargetName = name;
            return this;
        }

        public TargetBuilder SetSpend(double spend)
        {
            this.Spend = spend;
            return this;
        }

        public TargetBuilder SetTotalSum(double totalsum)
        {
            this.TotalSum = totalsum;
            return this;
        }

        public TargetBuilder SetCurrentSum(double currentSum)
        {
            this.CurrentSum = currentSum;
            return this;
        }

        public TargetBuilder setTargetTime(DateTime dateTime)
        {
            this.TargetTime = dateTime;
            return this;
        }

        public Target Build()
        {
            return this;
        }
    }
}

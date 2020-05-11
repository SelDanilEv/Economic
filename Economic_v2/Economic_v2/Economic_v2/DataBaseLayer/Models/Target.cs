using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.Models
{
    public class Target
    {
        public int Id { get; set; }
        
        public string TargetName { get; set; }
        public double Spend { get; set; }
        public double TotalSum { get; set; }
        public double CurrentSum { get; set; }
        public DateTime TargetTime { get; set; }

        [NotMapped]
        public string ShortTargetTime
        {
            get => TargetTime.ToShortDateString();
        }

        public TTarget CopyTo<TTarget>(TTarget destination) where TTarget : Target,new()
        {
            if (destination == null)
                destination = new TTarget();
            destination.Id = Id;
            destination.TargetName = TargetName;
            destination.Spend = Spend;
            destination.TotalSum = TotalSum;
            destination.CurrentSum = CurrentSum;
            destination.TargetTime = TargetTime;
            return destination;
        }

        #region constructors
        public Target()
        {
        }
        #endregion constuctors

    }
}

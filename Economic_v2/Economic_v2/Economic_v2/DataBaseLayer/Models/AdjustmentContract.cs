using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.Models
{
    public class AdjustmentContract
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }
        public string Type { get; set; }
        public int TypeId { get; set; }
        public Adjustment Adjustment { get; set; }


        #region constructors
        public AdjustmentContract()
        {
        }

        public AdjustmentContract(int id, string categoryName, string type, int typeId, Adjustment adjustment)
        {
            Id = id;
            CategoryName = categoryName;
            Type = type;
            TypeId = typeId;
            Adjustment = adjustment;
        }
        #endregion constructors

    }
}

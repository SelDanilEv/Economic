using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }
        public double Spend { get; set; }

        #region constructors
        public Category()
        {
        }

        public Category(Category category)
        {
            this.Id = category.Id;
            this.CategoryName = category.CategoryName;
            this.Spend = category.Spend;
        }

        public Category(string categoryName,  double spend)
        {
            CategoryName = categoryName;
            Spend = spend;
        }
        #endregion constructors

    }
}

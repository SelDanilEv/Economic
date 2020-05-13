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

        public Category(Category category):
            this(category.CategoryName,category.Spend)
        {
            this.Id = category.Id;
        }

        public Category(string categoryName,  double spend)
        {
            CategoryName = categoryName;
            Spend = spend;
        }
        #endregion constructors

    }
}

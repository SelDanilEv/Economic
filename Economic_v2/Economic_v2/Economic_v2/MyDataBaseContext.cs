using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2
{
    class MyDataBaseContext : DbContext
    {
        public MyDataBaseContext() : base("Economic_V1"){}

        public DbSet<User> Users { get; set; }
        public DbSet<Target> Targets{ get; set; }
        public DbSet<Category> Categories{ get; set; }
        public DbSet<Income> Incomes{ get; set; }
    }
}

using Economic_v2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.DataBaseLayer
{
    public class EconoMiCDBContext:DbContext             // data base context
    {
        public EconoMiCDBContext() : base() { }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Nodes { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<ActiveTarget>  ActiveTargets{ get; set; }
        public DbSet<OldTarget> OldTargets{ get; set; }
        public DbSet<Target> Targets{ get; set; }
        public DbSet<SuspendedTarget> SuspendedTargets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Adjustment> Adjustments { get; set; }
        public DbSet<AdjustmentContract> AdjustmentContracts { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<Tip> Tips { get; set; }
    }
}

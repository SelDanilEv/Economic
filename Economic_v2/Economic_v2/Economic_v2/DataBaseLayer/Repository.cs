using Economic_v2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.DataBaseLayer
{
    interface IRepository<T> where T : class                //use pattern repository for each entity
    {
        List<T> GetAll();
        T Get(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }

    public class UserRepository : IRepository<User>
    {
        private EconoMiCDBContext db;

        public UserRepository(EconoMiCDBContext context)
        {
            this.db = context;
        }

        public List<User> GetAll()
        {
            db.Incomes.Load();
            db.Targets.Load();
            db.Transactions.Load();
            db.Categories.Load();
            db.Nodes.Load();
            db.OldTargets.Load();
            db.SuspendedTargets.Load();
            db.ActiveTargets.Load();
            db.Statistics.Load();
            return db.Users.ToList();
        }

        public User Get(int id)
        {
            return db.Users.Find(id);
        }

        public User GetPure(string login)
        {
            User user = null;
            try
            {
                user = db.Users.ToList().Find(x => x.Login == login);
            }
            catch
            {
                user = new User() { Id = -1 };
            }
            finally
            {
                if (user == null)
                    user = new User() { Id = 0 };
            }
            return user;
        }

        public User Get(string login)
        {
            db.Incomes.Load();
            db.Targets.Load();
            db.Transactions.Load();
            db.AdjustmentContracts.Load();
            db.Adjustments.Load();
            db.Categories.Load();
            db.Nodes.Load();
            db.OldTargets.Load();
            db.SuspendedTargets.Load();
            db.ActiveTargets.Load();
            return GetAll().Find(x => x.Login == login);
        }

        public void Create(User User)
        {
            db.Users.Add(User);
        }

        public void Update(User User)
        {
            db.Entry(User).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            User User = db.Users.Find(id);
            if (User != null)
                db.Users.Remove(User);
        }
    }

    public class StatisticRepository : IRepository<Statistic>
    {
        private EconoMiCDBContext db;

        public StatisticRepository(EconoMiCDBContext context)
        {
            this.db = context;
        }

        public List<Statistic> GetAll()
        {
            db.Incomes.Load();
            db.Transactions.Load();
            db.Categories.Load();
            db.Nodes.Load();
            db.OldTargets.Load();
            db.SuspendedTargets.Load();
            db.ActiveTargets.Load();
            db.Statistics.Load();
            return db.Statistics.ToList();
        }

        public Statistic Get(int id)
        {
            return db.Statistics.Find(id);
        }

        public void Create(Statistic Statistic)
        {
            db.Statistics.Add(Statistic);
        }

        public void Update(Statistic Statistic)
        {
            db.Entry(Statistic).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Statistic Statistic = db.Statistics.Find(id);
            if (Statistic != null)
                db.Statistics.Remove(Statistic);
        }
    }

    public class TransactionRepository : IRepository<Transaction>
    {
        private EconoMiCDBContext db;

        public TransactionRepository(EconoMiCDBContext context)
        {
            this.db = context;
        }

        public List<Transaction> GetAll()
        {
            return db.Transactions.ToList();
        }

        public Transaction Get(int id)
        {
            return db.Transactions.Find(id);
        }

        public void Create(Transaction Transaction)
        {
            db.Transactions.Add(Transaction);
        }

        public void Update(Transaction Transaction)
        {
            db.Entry(Transaction).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Transaction Transaction = db.Transactions.Find(id);
            if (Transaction != null)
                db.Transactions.Remove(Transaction);
        }
    }

    public class NodeRepository : IRepository<Note>
    {
        private EconoMiCDBContext db;

        public NodeRepository(EconoMiCDBContext context)
        {
            this.db = context;
        }

        public List<Note> GetAll()
        {
            return db.Nodes.ToList();
        }

        public Note Get(int id)
        {
            return db.Nodes.Find(id);
        }

        public void Create(Note Node)
        {
            db.Nodes.Add(Node);
        }

        public void Update(Note Node)
        {
            db.Entry(Node).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Note Node = db.Nodes.Find(id);
            if (Node != null)
                db.Nodes.Remove(Node);
        }
    }

    public class TipRepository : IRepository<Tip>
    {
        private EconoMiCDBContext db;

        public TipRepository(EconoMiCDBContext context)
        {
            this.db = context;
        }

        public List<Tip> GetAll()
        {
            return db.Tips.ToList();
        }

        public Tip Get(int id)
        {
            return db.Tips.Find(id);
        }

        public void Create(Tip Tip)
        {
            db.Tips.Add(Tip);
        }

        public void Update(Tip Tip)
        {
            db.Entry(Tip).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Tip Tip = db.Tips.Find(id);
            if (Tip != null)
                db.Tips.Remove(Tip);
        }
    }

    public class AdjustmentRepository : IRepository<Adjustment>
    {
        private EconoMiCDBContext db;

        public AdjustmentRepository(EconoMiCDBContext context)
        {
            this.db = context;
        }

        public List<Adjustment> GetAll()
        {
            return db.Adjustments.ToList();
        }

        public Adjustment Get(int id)
        {
            return db.Adjustments.Find(id);
        }

        public void Create(Adjustment Adjustment)
        {
            db.Adjustments.Add(Adjustment);
        }

        public void Update(Adjustment Adjustment)
        {
            db.Entry(Adjustment).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Adjustment Adjustment = db.Adjustments.Find(id);
            if (Adjustment != null)
                db.Adjustments.Remove(Adjustment);
        }
    }

    public class AdjustmentContractRepository : IRepository<AdjustmentContract>
    {
        private EconoMiCDBContext db;

        public AdjustmentContractRepository(EconoMiCDBContext context)
        {
            this.db = context;
        }

        public List<AdjustmentContract> GetAll()
        {
            return db.AdjustmentContracts.ToList();
        }

        public AdjustmentContract Get(int id)
        {
            return db.AdjustmentContracts.Find(id);
        }

        public void Create(AdjustmentContract AdjustmentContract)
        {
            db.AdjustmentContracts.Add(AdjustmentContract);
        }

        public void Update(AdjustmentContract AdjustmentContract)
        {
            db.Entry(AdjustmentContract).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            AdjustmentContract AdjustmentContract = db.AdjustmentContracts.Find(id);
            if (AdjustmentContract != null)
                db.AdjustmentContracts.Remove(AdjustmentContract);
        }
    }

    public class SuspendedTargetRepository : IRepository<SuspendedTarget>
    {
        private EconoMiCDBContext db;

        public SuspendedTargetRepository(EconoMiCDBContext context)
        {
            this.db = context;
        }

        public List<SuspendedTarget> GetAll()
        {
            return db.SuspendedTargets.ToList();
        }

        public SuspendedTarget Get(int id)
        {
            return db.SuspendedTargets.Find(id);
        }

        public void Create(SuspendedTarget SuspendedTarget)
        {
            db.SuspendedTargets.Add(SuspendedTarget);
        }

        public void Update(SuspendedTarget SuspendedTarget)
        {
            db.Entry(SuspendedTarget).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            SuspendedTarget SuspendedTarget = db.SuspendedTargets.Find(id);
            if (SuspendedTarget != null)
                db.SuspendedTargets.Remove(SuspendedTarget);
        }
    }

    public class ActiveTargetRepository : IRepository<ActiveTarget>
    {
        private EconoMiCDBContext db;

        public ActiveTargetRepository(EconoMiCDBContext context)
        {
            this.db = context;
        }

        public List<ActiveTarget> GetAll()
        {
            return db.ActiveTargets.ToList();
        }

        public ActiveTarget Get(int id)
        {
            return db.ActiveTargets.Find(id);
        }

        public void Create(ActiveTarget Target)
        {
            db.ActiveTargets.Add(Target);
        }

        public void Update(ActiveTarget Target)
        {
            db.Entry(Target).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            ActiveTarget Target = db.ActiveTargets.Find(id);
            if (Target != null)
                db.ActiveTargets.Remove(Target);
        }
    }

    public class OldTargetRepository : IRepository<OldTarget>
    {
        private EconoMiCDBContext db;

        public OldTargetRepository(EconoMiCDBContext context)
        {
            this.db = context;
        }

        public List<OldTarget> GetAll()
        {
            return db.OldTargets.ToList();
        }

        public OldTarget Get(int id)
        {
            return db.OldTargets.Find(id);
        }

        public void Create(OldTarget OldTarget)
        {
            db.OldTargets.Add(OldTarget);
        }

        public void Update(OldTarget OldTarget)
        {
            db.Entry(OldTarget).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            OldTarget OldTarget = db.OldTargets.Find(id);
            if (OldTarget != null)
                db.OldTargets.Remove(OldTarget);
        }
    }

    public class CategoryRepository : IRepository<Category>
    {
        private EconoMiCDBContext db;

        public CategoryRepository(EconoMiCDBContext context)
        {
            this.db = context;
        }

        public List<Category> GetAll()
        {
            return db.Categories.ToList();
        }

        public Category Get(int id)
        {
            return db.Categories.Find(id);
        }

        public void Create(Category Category)
        {
            db.Categories.Add(Category);
        }

        public void Update(Category Category)
        {
            db.Entry(Category).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Category Category = db.Categories.Find(id);
            if (Category != null)
                db.Categories.Remove(Category);
        }
    }

    public class IncomeRepository : IRepository<Income>
    {
        private EconoMiCDBContext db;

        public IncomeRepository(EconoMiCDBContext context)
        {
            this.db = context;
        }

        public List<Income> GetAll()
        {
            return db.Incomes.ToList();
        }

        public Income Get(int id)
        {
            return db.Incomes.Find(id);
        }

        public void Create(Income Income)
        {
            db.Incomes.Add(Income);
        }

        public void Update(Income Income)
        {
            db.Entry(Income).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Income Income = db.Incomes.Find(id);
            if (Income != null)
                db.Incomes.Remove(Income);
        }
    }
}

using System;

namespace Economic_v2.DataBaseLayer
{
    public class UnitOfWork : IDisposable               //use pattern Unit of work
    {
        private EconoMiCDBContext db = new EconoMiCDBContext();
        private UserRepository userRepository;
        private NodeRepository nodeRepository;
        private CategoryRepository categoryRepository;
        private ActiveTargetRepository activeTargetRepository;
        private OldTargetRepository oldTargetRepository;
        private SuspendedTargetRepository suspendedTargetRepository;
        private AdjustmentContractRepository adjustmentContractRepository;
        private AdjustmentRepository adjustmentRepository;
        private IncomeRepository incomeRepository;
        private TransactionRepository transactionRepository;
        private StatisticRepository statisticRepository;
        private TipRepository tipRepository;

        public UserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }

        public StatisticRepository Statistics
        {
            get
            {
                if (statisticRepository == null)
                    statisticRepository = new StatisticRepository(db);
                return statisticRepository;
            }
        }

        public OldTargetRepository OldTargets
        {
            get
            {
                if (oldTargetRepository == null)
                    oldTargetRepository = new OldTargetRepository(db);
                return oldTargetRepository;
            }
        }

        public SuspendedTargetRepository SuspendedTargets
        {
            get
            {
                if (suspendedTargetRepository == null)
                    suspendedTargetRepository = new SuspendedTargetRepository(db);
                return suspendedTargetRepository;
            }
        }

        public AdjustmentRepository Adjustments
        {
            get
            {
                if (adjustmentRepository == null)
                    adjustmentRepository = new AdjustmentRepository(db);
                return adjustmentRepository;
            }
        }


        public AdjustmentContractRepository AdjustmentContracts
        {
            get
            {
                if (adjustmentContractRepository == null)
                    adjustmentContractRepository = new AdjustmentContractRepository(db);
                return adjustmentContractRepository;
            }
        }

        public NodeRepository Notes
        {
            get
            {
                if (nodeRepository == null)
                    nodeRepository = new NodeRepository(db);
                return nodeRepository;
            }
        }

        public TipRepository Tips
        {
            get
            {
                if (tipRepository == null)
                    tipRepository = new TipRepository(db);
                return tipRepository;
            }
        }

        public CategoryRepository Categories
        {
            get
            {
                if (categoryRepository == null)
                    categoryRepository = new CategoryRepository(db);
                return categoryRepository;
            }
        }

        public ActiveTargetRepository ActiveTargets
        {
            get
            {
                if (activeTargetRepository == null)
                    activeTargetRepository = new ActiveTargetRepository(db);
                return activeTargetRepository;
            }
        }

        public IncomeRepository Incomes
        {
            get
            {
                if (incomeRepository == null)
                    incomeRepository = new IncomeRepository(db);
                return incomeRepository;
            }
        }

        public TransactionRepository Transactions
        {
            get
            {
                if (transactionRepository == null)
                    transactionRepository = new TransactionRepository(db);
                return transactionRepository;
            }
        }

        public void Save()
        {
            try
            {
                db.SaveChanges();
            }
            catch { }
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

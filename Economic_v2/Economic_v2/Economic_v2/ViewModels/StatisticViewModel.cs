using Economic_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.ViewModels
{
    public class StatisticViewModel : ViewModelBase
    {
        private static object _context;
        private Statistic _statistic;
        private User User;

        public StatisticViewModel()
        {
            _context = this;
            User = MainViewModel.GetContext.CurrentUser;
            MakeCalculate();
        }

        public static StatisticViewModel GetContext
        {
            get => (StatisticViewModel)_context;
        }

        #region Caclulate
        private double GetIncomes()
        {
            double rv = 0;
            if (User.Incomes != null)
                foreach (Income income in User.Incomes)
                {
                    rv += income.Money;
                }
            return rv;
        }

        private double GetTargets()
        {
            double rv = 0;
            if (User.ActiveTargets != null)
                foreach (ActiveTarget target in User.ActiveTargets)
                {
                    rv += target.Spend;
                }
            return rv;
        }

        private double GetCategories()
        {
            double rv = 0;
            if (User.Categories != null)
                foreach (Category category in User.Categories)
                {
                    rv += category.Spend;
                }
            return rv;
        }

        public void MakeCalculate()
        {
            (new Task(() =>
            {
                User = MainViewModel.GetContext.CurrentUser;
                _statistic = User.Statistic;
                LargestTargetName = "Largest achieved goal";
                Target1Name = "Largest active goal";
                Target2Vis = Target3Vis = "Hidden";

                double incomeValue = GetIncomes();
                double categoryValue = GetCategories();
                double targetValue = GetTargets() + categoryValue;

                MaxFirstProgressBar = Math.Max(incomeValue, targetValue);

                _incomeProgressBar = (incomeValue / MaxFirstProgressBar) * 100 - 0.0001;
                _categoryProgressBar = (categoryValue / MaxFirstProgressBar) * 100 - 0.0001;
                _targetProgressBar = (targetValue / MaxFirstProgressBar) * 100 - 0.0001;

                _numberOfGoals = _statistic.CounterTargets;

                int NumberOfTargets = 3;
                double LargestTargetCostCur = 0;
                if (User.ActiveTargets != null)
                    if (User.ActiveTargets.Count < 3)
                        NumberOfTargets = User.ActiveTargets.Count;
                List<ActiveTarget> targets = new List<ActiveTarget>();
                if (User.ActiveTargets != null)
                    targets.AddRange(User.ActiveTargets);
                targets = targets.OrderByDescending(x => x.TotalSum).Take(NumberOfTargets).ToList();
                if (targets.Count != 0)
                    LargestTargetCostCur = targets[0].TotalSum;
                switch (targets.Count)
                {
                    case 3:
                    Target2Vis = Target3Vis = "Visible";
                    Target1Name = targets[0].TargetName;
                    Target2Name = targets[1].TargetName;
                    Target3Name = targets[2].TargetName;
                        break;
                    case 2:
                        Target2Vis = "Visible";
                        Target1Name = targets[0].TargetName;
                        Target2Name = targets[1].TargetName;
                        break;
                    case 1:
                        Target1Name = targets[0].TargetName;
                        break;
                }
                while (targets.Count < 3)
                {
                    targets.Add(new ActiveTarget() { TotalSum = 0 });
                }

                double LargestTargetCost = 0;
                if (_statistic.LargestTarget != null)
                    LargestTargetCost = _statistic.LargestTarget.TotalSum;

                if (LargestTargetCost != 0)
                    LargestTargetName = _statistic.LargestTarget.TargetName;

                MaxSecondProgressBar = Math.Max(LargestTargetCostCur, LargestTargetCost);

                _largestTarget = (LargestTargetCost / MaxSecondProgressBar) * 100 - 0.0001;
                _target1 = (targets[0].TotalSum / MaxSecondProgressBar) * 100 - 0.0001;
                _target2 = (targets[1].TotalSum / MaxSecondProgressBar) * 100 - 0.0001;
                _target3 = (targets[2].TotalSum / MaxSecondProgressBar) * 100 - 0.0001;


                NotifyPropertyChanged("IncomeProgressBar");
                NotifyPropertyChanged("CategoryProgressBar");
                NotifyPropertyChanged("TargetProgressBar");
                NotifyPropertyChanged("NumberOfGoals");
                NotifyPropertyChanged("LargestTarget");
                NotifyPropertyChanged("Target1");
                NotifyPropertyChanged("Target2");
                NotifyPropertyChanged("Target3");
                NotifyPropertyChanged("LargestTargetName");
                NotifyPropertyChanged("Target1Name");
                NotifyPropertyChanged("Target2Name");
                NotifyPropertyChanged("Target3Name");
                NotifyPropertyChanged("Target2Vis");
                NotifyPropertyChanged("Target3Vis");
            })).Start();
        }
        #endregion


        #region View
        private double MaxFirstProgressBar;
        private double MaxSecondProgressBar;

        private double _incomeProgressBar;
        public double IncomeProgressBar
        {
            get => _incomeProgressBar;
            set => _incomeProgressBar = value;
        }

        private double _categoryProgressBar;
        public double CategoryProgressBar
        {
            get => _categoryProgressBar;
            set => _categoryProgressBar = value;
        }

        private double _targetProgressBar;
        public double TargetProgressBar
        {
            get => _targetProgressBar;
            set => _targetProgressBar = value;
        }

        private int _numberOfGoals;
        public int NumberOfGoals
        {
            get => _numberOfGoals;
            set => _numberOfGoals = value;
        }


        private double _largestTarget;
        public double LargestTarget
        {
            get => _largestTarget;
            set => _largestTarget = value;
        }

        private double _target1;
        public double Target1
        {
            get => _target1;
            set => _target1 = value;
        }

        private double _target2;
        public double Target2
        {
            get => _target2;
            set => _target2 = value;
        }

        private double _target3;
        public double Target3
        {
            get => _target3;
            set => _target3 = value;
        }

        public string LargestTargetName { get; set; }
        public string Target1Name { get; set; }
        public string Target2Name { get; set; }
        public string Target3Name { get; set; }

        public string Target2Vis { get; set; }
        public string Target3Vis { get; set; }
        #endregion
    }
}

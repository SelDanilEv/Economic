using Economic_v2.DataBaseLayer;
using Economic_v2.Commands;
using System.Windows.Input;
using System.Collections.Generic;
using Economic_v2.Models;
using System.Windows;
using System;
using System.Threading;
using Economic_v2.Windows;
using Economic_v2.Help;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Economic_v2.Builders;

namespace Economic_v2.ViewModels
{
    public class TargetsListViewViewModel : ViewModelBase
    {
        public TargetsListViewViewModel()
        {

        }

        public void ReadTargets()   // get actual notes
        {
            List<Target> targets = new List<Target>();

            switch (Mode)
            {
                case 0:
                    if (MainViewModel.GetContext.CurrentUser.ActiveTargets == null)
                        MainViewModel.GetContext.CurrentUser.ActiveTargets = new List<ActiveTarget>();
                    List<ActiveTarget> UserTargetsA = MainViewModel.GetContext.CurrentUser.ActiveTargets;
                    if (UserTargetsA != null)
                    {
                        foreach (Target target in UserTargetsA)
                        {
                            targets.Add(target);
                        }
                    }
                    break;
                case 1:
                    if (MainViewModel.GetContext.CurrentUser.SuspendedTargets == null)
                        MainViewModel.GetContext.CurrentUser.SuspendedTargets = new List<SuspendedTarget>();
                    List<SuspendedTarget> UserTargetsS = MainViewModel.GetContext.CurrentUser.SuspendedTargets;
                    if (UserTargetsS != null)
                    {
                        foreach (Target target in UserTargetsS)
                        {
                            targets.Add(target);
                        }
                    }
                    break;
                case 2:
                    if (MainViewModel.GetContext.CurrentUser.OldTargets == null)
                        MainViewModel.GetContext.CurrentUser.OldTargets = new List<OldTarget>();
                    List<OldTarget> UserTargetsO = MainViewModel.GetContext.CurrentUser.OldTargets;
                    if (UserTargetsO != null)
                    {
                        foreach (Target target in UserTargetsO)
                        {
                            targets.Add(target);
                        }
                    }
                    break;
            }

            while (targets.Count < 9)     //make empty notes to make minimem 10
            {
                targets.Add(new TargetBuilder().
                    SetName("Empty").
                    setTargetTime(DateTime.Today.AddDays(1)).
                    SetSpend(0).Build());
            }
            Targets = targets;
        }

        #region View
        public void NotifyTargetList()
        {
            NotifyPropertyChanged("TargetsObs");
        }

        public ObservableCollection<Target> TargetsObs
        {
            get
            {
                return new ObservableCollection<Target>(Targets);
            }
        }

        private List<Target> _targets;
        public List<Target> Targets
        {
            get { ReadTargets(); return _targets; }
            set { _targets = value; }
        }

        private int _mode = 0;//0-active ,1-suspended ,2-old
        public int Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;

                SelectedTarget = null;

                NotifyPropertyChanged("ActiveFlag");
                NotifyPropertyChanged("SuspendedFlag");
                NotifyPropertyChanged("OldFlag");
                NotifyPropertyChanged("TargetsObs");
            }
        }

        public bool ActiveFlag
        {
            get
            {
                return _mode == 0;
            }
            set
            {
                Mode = 0;
            }
        }

        public bool SuspendedFlag
        {
            get
            {
                return _mode == 1;
            }
            set
            {
                Mode = 1;
            }
        }

        public bool OldFlag
        {
            get
            {
                return _mode == 2;
            }
            set
            {
                Mode = 2;
            }
        }



        private Target _selectedTarget { get; set; }
        public Target SelectedTarget
        {
            get
            {
                return _selectedTarget;
            }
            set
            {
                _selectedTarget = value;
                TargetsPageViewModel.Model.NotifyEndableButtonsEditAndDelete();
            }
        }

        public bool IsSelectedTarget
        {
            get
            {
                if (_selectedTarget == null)
                    return false;
                return true;
            }
        }
        #endregion

        #region ImplementationCommand
        public void OnDeleteTarget(Target target, int mode)             //call via context
        {
            switch (mode)
            {
                case 0:
                    MainViewModel.GetContext.CurrentUser.ActiveTargets.RemoveAll(
                                     x => x.Id == target.Id);
                    break;
                case 1:
                    MainViewModel.GetContext.CurrentUser.SuspendedTargets.RemoveAll(
                                     x => x.Id == target.Id);
                    break;
                case 2:
                    MainViewModel.GetContext.CurrentUser.OldTargets.RemoveAll(
                                     x => x.Id == target.Id);
                    break;
            }

            if (target.CurrentSum == target.TotalSum)
            {
                MainViewModel.GetContext.CurrentUser.Total_money -= target.TotalSum;
                if (MainViewModel.GetContext.CurrentUser.Statistic.LargestTarget.TotalSum <
                    target.TotalSum)
                    MainViewModel.GetContext.CurrentUser.Statistic.LargestTarget = target;
                MainViewModel.GetContext.CurrentUser.Statistic.CounterTargets++;
                StatisticViewModel.GetContext.MakeCalculate();
            }
            MainViewModel.GetContext.CurrentUser.Reserve_money -= target.CurrentSum;
            UnitOfWorkSingleton.GetUnitOfWork.Users.Update(MainViewModel.GetContext.CurrentUser);
            UnitOfWorkSingleton.GetUnitOfWork.Save();
            NotifyPropertyChanged("TargetsObs");
            MainViewModel.GetContext.UpdateInfo();
        }
        #endregion
    }
}

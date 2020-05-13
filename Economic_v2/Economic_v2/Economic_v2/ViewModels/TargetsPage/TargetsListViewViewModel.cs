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
            if (MainViewModel.CurrentUser.ActiveTargets == null)
                MainViewModel.CurrentUser.ActiveTargets = new List<ActiveTarget>();
            List<ActiveTarget> UserTargets = MainViewModel.CurrentUser.ActiveTargets;
            List<Target> targets = new List<Target>();
            if (UserTargets != null)
            {
                foreach (Target target in UserTargets)
                {
                    targets.Add(target);
                }
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
            set { _targets = value;}
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
        public void OnDeleteTarget(Target target)             //call via context
        {
            MainViewModel.CurrentUser.ActiveTargets.RemoveAll(
                x => x.Id == target.Id);
            UnitOfWorkSingleton.GetUnitOfWork.Users.Update(MainViewModel.CurrentUser);
            UnitOfWorkSingleton.GetUnitOfWork.Save();
            NotifyPropertyChanged("TargetsObs");
        }
        #endregion
    }
}

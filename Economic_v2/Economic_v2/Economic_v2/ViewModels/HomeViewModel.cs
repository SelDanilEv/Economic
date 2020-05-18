using Economic_v2.Commands;
using Economic_v2.DataBaseLayer;
using Economic_v2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private static object _homeContext;

        public HomeViewModel()
        {
            _homeContext = this;
            (new Task(() =>
            {
                Tips = UnitOfWorkSingleton.GetUnitOfWork.Tips.GetAll();
                if (Tips != null)
                {
                    GetNextTip();
                }
                NotifyPropertyChanged("CurTip");
            })).Start();
        }

        public static HomeViewModel GetContext
        {
            get => (HomeViewModel)_homeContext;
        }


        public void ReadTargets()   // get actual notes
        {
            List<Target> targets = new List<Target>();

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
            Targets = targets;
        }

        public void FindLargestTarget()
        {
            ActiveTarget activeTarget = new ActiveTarget() { TotalSum = 1 };
            if (MainViewModel.GetContext.CurrentUser.ActiveTargets != null)
            {
                if (MainViewModel.GetContext.CurrentUser.ActiveTargets.Count != 0)
                {
                    activeTarget = MainViewModel.GetContext.CurrentUser.ActiveTargets.OrderByDescending(x => x.TotalSum).ToList()[0];
                }
            }
            LargestTarget = activeTarget;
            NotifyPropertyChanged("LargestTargetName");
            NotifyPropertyChanged("ProgressStr");
            NotifyPropertyChanged("ProgressPercent");
            NotifyPropertyChanged("VisibilityProgress");
            NotifyPropertyChanged("ProgressHeight");
        }

        #region View
        public void NotifyTargetList()
        {
            NotifyPropertyChanged("FinishedTargetsObs");
        }

        private List<Tip> Tips;

        public Tip _tip = new Tip();
        public Tip CurTip
        {
            get
            {
                if (Tips != null)
                {
                    return _tip;
                }
                return new Tip();
            }
            set
            {
            }
        }

        public ObservableCollection<Target> FinishedTargetsObs
        {
            get
            {
                return new ObservableCollection<Target>(Targets);
            }
        }

        private List<Target> _targets = new List<Target>();
        public List<Target> Targets
        {
            get { if (MainViewModel.GetContext != null) ReadTargets(); return _targets; }
            set { _targets = value; }
        }

        private ActiveTarget LargestTarget = new ActiveTarget() { TotalSum = 1 };
        public string LargestTargetName
        {
            get => LargestTarget.TargetName;
        }
        public double ProgressPercent
        {
            get => RoundUp(LargestTarget.CurrentSum / LargestTarget.TotalSum, 3)*100;
        }
        public string ProgressStr
        {
            get => ProgressPercent.ToString() + "%";
        }

        public string VisibilityProgress
        {
            get
            {
                if (LargestTarget.Id == 0)
                {
                    _progressHeight = 0;
                    return "Hidden";
                }
                _progressHeight = 70;
                return "Visible";
            }
        }

        private double _progressHeight = 70;
        public double ProgressHeight
        {
            get
            {
                return _progressHeight;
            }
        }
        #endregion


        #region Calc
        private double RoundUp(double number, int digits)
        {
            var factor = Convert.ToDouble(Math.Pow(10, digits));
            return Math.Ceiling(number * factor) / factor;
        }
        #endregion


        #region Commands
        private Command NextTipCommand;
        public Command NextTip
        {
            get
            {
                if (NextTipCommand == null)
                {
                    NextTipCommand = new Command(GetNextTip);
                }
                return NextTipCommand;
            }
        }

        private Command PreviousTipCommand;
        public Command PreviousTip
        {
            get
            {
                if (PreviousTipCommand == null)
                {
                    PreviousTipCommand = new Command(GetPreviousTip);
                }
                return PreviousTipCommand;
            }
        }
        #endregion

        #region ImplementationCommand
        private int counter = -1;  // in start program onvoke GetNextTip => counter =0;
        private void GetNextTip()
        {
            if (Tips != null)
            {
                if (++counter == Tips.Count)
                    counter = 0;
                Tip tip = Tips[counter];
                _tip = tip;
                NotifyPropertyChanged("CurTip");
            }
        }
        private void GetPreviousTip()
        {
            if (Tips != null)
            {
                if (--counter == -1)
                    counter = Tips.Count - 1;
                Tip tip = Tips[counter];
                _tip = tip;
                NotifyPropertyChanged("CurTip");
            }
        }
        #endregion

    }
}

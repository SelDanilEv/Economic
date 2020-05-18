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
using Economic_v2.Builders;
using Economic_v2.Logic;

namespace Economic_v2.ViewModels
{
    public class AddOrEditTargetsViewModel : ViewModelBase
    {
        public AddOrEditTargetsViewModel()
        {
            Initial();
        }

        #region Setup
        public void Initial()
        {
            if (!TargetsPageViewModel.IsEdit)   //make start settings
            {
                ReturnedTarget = new TargetBuilder();
                IfCreate();
            }
            else
            {
                ReturnedTarget = new TargetBuilder(TargetsPageViewModel.SelectedTarget);

                if (ReturnedTarget.Id == 0)
                {
                    IfCreate();
                }
                else
                {
                    IfEdit();
                }
            }
            _timePriority = false;

            NotifyPropertyChanged("TimePriority");
            NotifyPropertyChanged("TargetName");
            NotifyPropertyChanged("TotalSum");
            NotifyPropertyChanged("CurrentSum");
            NotifyPropertyChanged("Spend");
            NotifyPropertyChanged("TargetTime");
            NotifyPropertyChanged("TargetNameError");
            NotifyPropertyChanged("TotalSumError");
            NotifyPropertyChanged("CurrentSumError");
            NotifyPropertyChanged("SpendError");
            NotifyPropertyChanged("TargetTimeError");
        }

        public void IfCreate()
        {
            _targetName = "";
            totalsumvalue = currentsumvalue = spendvalue = null;
            _targetTime = DateTime.Today.AddDays(1);

            _totalSum = _currentSum = _spend = Double.PositiveInfinity;
            SpendError = TargetNameError = CurrentSumError =
                TotalSumError = "Requared value";

            TargetTimeError = null;
        }

        public void IfEdit()
        {
            _targetName = ReturnedTarget.TargetName;
            if (ReturnedTarget.TargetTime != null)
                _targetTime = ReturnedTarget.TargetTime;
            else
                _targetTime = DateTime.Today.AddDays(1);
            _totalSum = ReturnedTarget.TotalSum;
            _currentSum = ReturnedTarget.CurrentSum;
            _spend = ReturnedTarget.Spend;

            SpendError = TargetNameError = CurrentSumError =
                TotalSumError = TargetTimeError = null;
        }
        #endregion

        #region View
        private bool _timePriority = false;
        public bool TimePriority { get => _timePriority; set => _timePriority = value; }


        private string _targetName;
        public string TargetNameError { get; set; }
        public string TargetName
        {
            get
            {
                return _targetName;
            }
            set
            {
                _targetName = value;
                validateTargetName();
                NotifyPropertyChanged("TargetNameError");
            }
        }

        private double _totalSum;
        public string TotalSumError { get; set; }
        private string totalsumvalue;
        public string TotalSum
        {
            get
            {
                if (TotalSumError == null)
                    return _totalSum.ToString();
                else
                    return totalsumvalue;
            }
            set
            {
                try
                {
                    _totalSum = double.Parse(value);
                    validateTotalSum();
                    validateCurrentSum();
                    validateSpend();
                    NotifyPropertyChanged("CurrentSumError");
                    NotifyPropertyChanged("SpendError");
                    NotifyPropertyChanged("TotalSumError");
                    TotalSumError = null;
                }
                catch
                {
                    totalsumvalue = value;
                    TotalSumError = "Value must be number";
                    NotifyPropertyChanged("TotalSumError");
                }
            }
        }

        private double _currentSum;
        public string CurrentSumError { get; set; }
        private string currentsumvalue;
        public string CurrentSum
        {
            get
            {
                if (CurrentSumError == null)
                    return _currentSum.ToString();
                else
                    return currentsumvalue;
            }
            set
            {
                try
                {
                    _currentSum = double.Parse(value);
                    validateCurrentSum();
                    NotifyPropertyChanged("CurrentSumError");
                    CurrentSumError = null;
                }
                catch
                {
                    currentsumvalue = value;
                    CurrentSumError = "Value must be number";
                    NotifyPropertyChanged("CurrentSumError");
                }
            }
        }

        private double _spend;
        public string SpendError { get; set; }
        private string spendvalue;
        public string Spend
        {
            get
            {
                if (SpendError == null)
                    return _spend.ToString();
                else
                    return spendvalue;
            }
            set
            {
                try
                {
                    _spend = double.Parse(value);
                    validateSpend();
                    NotifyPropertyChanged("SpendError");
                    SpendError = null;
                }
                catch
                {
                    spendvalue = value;
                    SpendError = "Value must be number";
                    NotifyPropertyChanged("SpendError");
                }
            }
        }

        private DateTime _targetTime;
        public string TargetTimeError { get; set; }
        public DateTime TargetTime
        {
            get => _targetTime;
            set
            {
                _targetTime = value;
                validateTargetTime();
                NotifyPropertyChanged("TargetTimeError");
            }
        }
        #endregion

        #region GetTarget
        public bool SuccessfulValidation
        {
            get
            {
                if (_timePriority)
                    return validateAllTime();
                return validateAllSpend();
            }
        }

        private bool validateTargetName()
        {
            if (_targetName.Length < 2)
            {
                TargetNameError = "Target name too short";
                return false;
            }

            if (_targetName[0] == ' ')
            {
                TargetNameError = "Target name musn't started with space";
                return false;
            }

            ReturnedTarget.SetName(_targetName);
            TargetNameError = null;
            return true;
        }

        private bool validateTotalSum()
        {
            string[] temp = _totalSum.ToString().Split('.', ',');
            if (temp.Length > 1)
            {
                if (temp[1].Length > 2)
                {
                    TotalSumError = "Money format have two decimal places";
                    return false;
                }
            }

            if (_totalSum < 0)
            {
                TotalSumError = "Total sum must be positive";
                return false;
            }

            if (_totalSum == Double.PositiveInfinity)
            {
                TotalSumError = "Requared value";
                return false;
            }

            ReturnedTarget.SetTotalSum((double)_totalSum);
            TotalSumError = null;
            return true;
        }

        private bool validateCurrentSum()
        {
            string[] temp = _currentSum.ToString().Split('.', ',');
            if (temp.Length > 1)
            {
                if (temp[1].Length > 2)
                {
                    CurrentSumError = "Money format have two decimal places";
                    return false;
                }
            }

            if (_currentSum < 0)
            {
                CurrentSumError = "Current sum must be positive";
                return false;
            }

            if (_currentSum == Double.PositiveInfinity)
            {
                CurrentSumError = "Requared value";
                return false;
            }

            if (_currentSum > _totalSum)
            {
                CurrentSumError = "Current sum must be less or equal than total sum";
                return false;
            }

            ReturnedTarget.SetCurrentSum((double)_currentSum);
            CurrentSumError = null;
            return true;
        }

        private bool validateSpend()
        {
            string[] temp = _spend.ToString().Split('.', ',');
            if (temp.Length > 1)
            {
                if (temp[1].Length > 2)
                {
                    SpendError = "Money format have two decimal places";
                    return false;
                }
            }

            if (_spend < 0)
            {
                SpendError = "Monthly expenses must be positive";
                return false;
            }

            if (_spend == Double.PositiveInfinity)
            {
                SpendError = "Requared value";
                return false;
            }

            if (_spend > _totalSum)
            {
                SpendError = "Monthly expenses must be less or equal than total sum";
                return false;
            }

            ReturnedTarget.SetSpend((double)_spend);
            SpendError = null;
            return true;
        }

        private bool validateTargetTime()
        {
            switch (DateTime.Compare(_targetTime.Date, DateTime.Today.Date))
            {
                case 1:
                    if(_targetTime.Month== DateTime.Today.Month)
                    {
                        TargetTimeError = "Pick future month";
                        return false;
                    }
                    ReturnedTarget.setTargetTime(_targetTime);
                    TargetTimeError = null;
                    return true;
                default:
                    TargetTimeError = "Pick future date";
                    return false;
            }
        }

        public bool validateAllTime()
        {
            bool rv = validateTargetName() &&
            validateTotalSum() &&
            validateCurrentSum() &&
            validateTargetTime();

            NotifyPropertyChanged("TargetNameError");
            NotifyPropertyChanged("TotalSumError");
            NotifyPropertyChanged("CurrentSumError");
            NotifyPropertyChanged("TargetTimeError");
            return rv;
        }

        public bool validateAllSpend()
        {
            bool rv = validateTargetName() &&
            validateTotalSum() &&
            validateCurrentSum() &&
            validateSpend();

            NotifyPropertyChanged("TargetNameError");
            NotifyPropertyChanged("TotalSumError");
            NotifyPropertyChanged("CurrentSumError");
            NotifyPropertyChanged("SpendError");
            return rv;
        }

        public bool validateAll()
        {
            bool rv = validateTargetName() &&
            validateTotalSum() &&
            validateCurrentSum() &&
            validateSpend() &&
            validateTargetTime();

            NotifyPropertyChanged("TargetNameError");
            NotifyPropertyChanged("TotalSumError");
            NotifyPropertyChanged("CurrentSumError");
            NotifyPropertyChanged("SpendError");
            NotifyPropertyChanged("TargetTimeError");
            return rv;
        }

        public TargetBuilder ReturnedTarget;

        public Target GetTarget
        {
            get
            {
                if (SuccessfulValidation)
                {
                    TargetCalcController controller = new TargetCalcController();
                    if (_timePriority)
                        return controller.CalculateTargetWithTime(ReturnedTarget.Build());
                    else
                        return controller.CalculateTargetWithSpend(ReturnedTarget.Build());
                }
                return null;
            }
        }

        #endregion

    }
}

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

namespace Economic_v2.ViewModels
{
    public class AddOrEditIncomesViewModel : ViewModelBase
    {
        public AddOrEditIncomesViewModel()
        {
            Initial();
        }

        #region Setup
        public void Initial()
        {
            if (!IncomesPageViewModel.IsEdit)   //make start settings
            {
                ReturnedIncome = new Income();
                IfCreate();
            }
            else
            {
                ReturnedIncome = new Income(IncomesPageViewModel.SelectedIncome);

                if (ReturnedIncome.Id == 0)
                {
                    IfCreate();
                }
                else
                {
                    IfEdit();
                }
            }

            NotifyPropertyChanged("IncomeNameError");
            NotifyPropertyChanged("MoneyError");
            NotifyPropertyChanged("DateError");
            ClearFields = false;
        }

        public void IfCreate()
        {
            _incomeName = "";
            moneyvalue = null;
            _targetTime = DateTime.Today.AddDays(1);
            _money = Double.PositiveInfinity;
            MoneyError = IncomeNameError= "Requared value";
        }

        public void IfEdit()
        {
            _incomeName = ReturnedIncome.IncomeName;
            if (ReturnedIncome.Date != null)
                _targetTime = ReturnedIncome.Date;
            else
                _targetTime = DateTime.Today.AddDays(1);
            _money = ReturnedIncome.Money;

            MoneyError = IncomeNameError = null;
        }
        #endregion

        #region View
        public bool ClearFields = false;

        private string _incomeName;
        public string IncomeNameError { get; set; }
        public string IncomeName
        {
            get
            {
                if (ClearFields)       //if need clear
                    new Task(() => Initial()).Start();
                return _incomeName;
            }
            set
            {
                _incomeName = value;
                validateIncomeName();
                NotifyPropertyChanged("IncomeNameError");
            }
        }

        private double _money;
        public string MoneyError { get; set; }
        private string moneyvalue;
        public string Money
        {
            get
            {
                if (MoneyError == null)
                    return _money.ToString();
                else
                    return moneyvalue;
            }
            set
            {
                try
                {
                    _money = double.Parse(value);
                    validateMoney();
                    NotifyPropertyChanged("MoneyError");
                    MoneyError = null;
                }
                catch
                {
                    moneyvalue = value;
                    MoneyError = "Value must be number";
                    NotifyPropertyChanged("MoneyError");
                }
            }
        }

        private DateTime _targetTime;
        public string DateError { get; set; }
        public DateTime Date
        {
            get => _targetTime;
            set
            {
                _targetTime = value;
                validateDate();
                NotifyPropertyChanged("DateError");
            }
        }
        #endregion

        #region GetIncome
        public bool SuccessfulValidation => validateAll();

        private bool validateIncomeName()
        {
            if (_incomeName.Length < 2)
            {
                IncomeNameError = "Income name too short";
                return false;
            }

            if (_incomeName[0] == ' ')
            {
                IncomeNameError = "Income name musn't started with space";
                return false;
            }

            ReturnedIncome.IncomeName = _incomeName;
            IncomeNameError = null;
            return true;
        }

        private bool validateMoney()
        {
            string[] temp = _money.ToString().Split('.', ',');
            if (temp.Length > 1)
            {
                if (temp[1].Length > 2)
                {
                    MoneyError = "Money format have two decimal places";
                    return false;
                }
            }

            if (_money <= 0)
            {
                MoneyError = "Monthly income must be positive";
                return false;
            }

            if (_money == Double.PositiveInfinity)
            {
                MoneyError = "Requared value";
                return false;
            }

            ReturnedIncome.Money = _money;
            MoneyError = null;
            return true;
        }

        private bool validateDate()
        {
            switch (DateTime.Compare(_targetTime, DateTime.Today))
            {
                case 1:
                    ReturnedIncome.Date = _targetTime;
                    DateError = null;
                    return true;
                default:
                    DateError = "Pick future date";
                    return false;
            }
        }

        public bool validateAll()
        {
            bool rv = validateIncomeName() &&
            validateMoney()&&validateDate() ;

            NotifyPropertyChanged("IncomeNameError");
            NotifyPropertyChanged("MoneyError");
            NotifyPropertyChanged("DateError");
            return rv;
        }

        public Income ReturnedIncome;

        public Income GetIncome
        {
            get
            {
                if (SuccessfulValidation)
                {
                    ClearFields = true;
                    return ReturnedIncome;
                }
                return null;
            }
        }
        #endregion

    }
}

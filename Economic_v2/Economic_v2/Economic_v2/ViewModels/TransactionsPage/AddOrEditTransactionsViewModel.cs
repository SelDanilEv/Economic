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
    public class AddOrEditTransactionsViewModel : ViewModelBase
    {
        public AddOrEditTransactionsViewModel()
        {
            Initial();
        }

        #region Setup
        public void Initial()
        {
            if (!TransactionsPageViewModel.IsEdit)   //make start settings
            {
                ReturnedTransaction = new Transaction() {Date = DateTime.Now };
                IfCreate();
            }
            else
            {
                ReturnedTransaction = new Transaction(TransactionsPageViewModel.SelectedTransaction);

                if (ReturnedTransaction.Id == 0)
                {
                    IfCreate();
                }
                else
                {
                    IfEdit();
                }
            }

            NotifyPropertyChanged("TransactionNameError");
            NotifyPropertyChanged("SpendError");
            NotifyPropertyChanged("DateError");
            ClearFields = false;
        }

        public void IfCreate()
        {
            _transactionName = "";
            spendvalue = null;
            _spend = Double.PositiveInfinity;
            SpendError = TransactionNameError = "Requared value";
        }

        public void IfEdit()
        {
            _transactionName = ReturnedTransaction.TransactionName;
            _spend = ReturnedTransaction.Spend;
            SpendError = TransactionNameError = null;
        }
        #endregion

        #region View
        public bool ClearFields = false;

        private string _transactionName;
        public string TransactionNameError { get; set; }
        public string TransactionName
        {
            get
            {
                if (ClearFields)       //if need clear
                    new Task(() => Initial()).Start();
                return _transactionName;
            }
            set
            {
                _transactionName = value;
                validateTransactionName();
                NotifyPropertyChanged("TransactionNameError");
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
        #endregion

        #region GetTransaction
        public bool SuccessfulValidation => validateAll();

        private bool validateTransactionName()
        {
            if (_transactionName.Length <= 2)
            {
                TransactionNameError = "Transaction name too short";
                return false;
            }

            if (_transactionName[0] == ' ')
            {
                TransactionNameError = "Transaction name musn't started with space";
                return false;
            }

            ReturnedTransaction.TransactionName = _transactionName;
            TransactionNameError = null;
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

            if (_spend <= 0)
            {
                SpendError = "Transaction cost must be positive";
                return false;
            }

            if (_spend == Double.PositiveInfinity)
            {
                SpendError = "Requared value";
                return false;
            }

            ReturnedTransaction.Spend = _spend;
            SpendError = null;
            return true;
        }

        public bool validateAll()
        {
            bool rv = validateTransactionName() &&
            validateSpend();

            NotifyPropertyChanged("TransactionNameError");
            NotifyPropertyChanged("SpendError");
            return rv;
        }

        public Transaction ReturnedTransaction;

        public Transaction GetTransaction
        {
            get
            {
                if (SuccessfulValidation)
                {
                    ClearFields = true;
                    return ReturnedTransaction;
                }
                return null;
            }
        }
        #endregion

    }
}

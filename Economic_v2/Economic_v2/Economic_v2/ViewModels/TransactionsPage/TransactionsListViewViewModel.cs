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
    public class TransactionsListViewViewModel : ViewModelBase
    {

        public TransactionsListViewViewModel()
        {
        }


        public void ReadTransactions()   // get actual notes
        {
            if (MainViewModel.GetContext.CurrentUser.Transactions == null)
                MainViewModel.GetContext.CurrentUser.Transactions = new List<Transaction>();
            List<Transaction> UserTransactions = new List<Transaction>( MainViewModel.GetContext.CurrentUser.Transactions);
           
            while (UserTransactions.Count < 7)     //make empty notes to make minimem 10
            {
                UserTransactions.Add(new Transaction("Empty",0,DateTime.Now));
            }
            Transactions = UserTransactions;
        }

        #region View
        public void NotifyTransactionList()
        {
            NotifyPropertyChanged("TransactionsObs");
        }

        public ObservableCollection<Transaction> TransactionsObs
        {
            get
            {
                return new ObservableCollection<Transaction>(Transactions);
            }
        }

        private List<Transaction> _transactions;

        public List<Transaction> Transactions
        {
            get { ReadTransactions(); return _transactions; }
            set { _transactions = value; }
        }


        private Transaction _selectedTransaction { get; set; }
        public Transaction SelectedTransaction
        {
            get
            {
                return _selectedTransaction;
            }
            set
            {
                _selectedTransaction = value;
                TransactionsPageViewModel.Model.NotifyEndableButtonsEditAndDelete();
            }
        }

        public bool IsSelectedTransaction
        {
            get
            {
                if (_selectedTransaction == null)
                    return false;
                return true;
            }
        }
        #endregion

        #region ImplementationCommand
        public void OnDeleteTransaction(Transaction target)             //call via context
        {
            MainViewModel.GetContext.CurrentUser.Transactions.RemoveAll(
                x => x.Id == target.Id);
            UnitOfWorkSingleton.GetUnitOfWork.Users.Update(MainViewModel.GetContext.CurrentUser);
            UnitOfWorkSingleton.GetUnitOfWork.Save();
            NotifyPropertyChanged("TransactionsObs");
        }
        #endregion
    }
}

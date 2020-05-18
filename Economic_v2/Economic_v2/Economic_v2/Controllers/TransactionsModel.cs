using Economic_v2.Builders;
using Economic_v2.DataBaseLayer;
using Economic_v2.Logic;
using Economic_v2.Pages;
using Economic_v2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.Models
{
    public class TransactionsModel
    {
        static object[] DataContexts = new object[3];         //here data context all controls where was Transaction
        object[] Views = new object[3];         //here Views
        UnitOfWork Data = UnitOfWorkSingleton.GetUnitOfWork;

        public TransactionsModel(object context)
        {
            Views[0] = new AddOrEditTransaction();
            Views[1] = new TransactionsListView();
            DataContexts[0] = ((AddOrEditTransaction)Views[0]).DataContext;
            DataContexts[1] = ((TransactionsListView)Views[1]).DataContext;
            DataContexts[2] = context;
        }


        #region Contexts
        public static AddOrEditTransactionsViewModel AddOrEditContext
        {
            get => (AddOrEditTransactionsViewModel)DataContexts[0];
        }

        public static TransactionsListViewViewModel ListContext
        {
            get => (TransactionsListViewViewModel)DataContexts[1];
        }

        public static TransactionsPageViewModel PageContext
        {
            get => (TransactionsPageViewModel)DataContexts[2];
        }
        #endregion

        #region Views
        public AddOrEditTransaction AddOrEditView
        {
            get => (AddOrEditTransaction)Views[0];
        }

        public TransactionsListView ListView
        {
            get => (TransactionsListView)Views[1];
        }

        public TransactionsPage PageView
        {
            get => (TransactionsPage)Views[2];
        }
        #endregion

        public void NotifyEndableButtonsEditAndDelete()
        {
            PageContext.NotifyEndableButtonsEditAndDelete();
        }

        public bool DismissDelete;
        public Transaction DeletedTransaction;
        public void DeleteTransaction()
        {
            new Task(() =>    //start method in context
            {
                ListContext.OnDeleteTransaction(new Transaction(DeletedTransaction));
                DeletedTransaction = null;
            }).Start();
        }

        public void ConfirmButton(bool isEdit)
        {
            new Task(() =>
            {
                if (MainViewModel.GetContext.CurrentUser.Transactions == null)  //if current not any note
                {
                    MainViewModel.GetContext.CurrentUser.Transactions = new List<Transaction>();
                }
                Transaction transaction = AddOrEditContext.GetTransaction;
                if (isEdit)
                {
                    try         //if edit just replace
                    {
                        MainViewModel.GetContext.CurrentUser.Transactions[MainViewModel.GetContext.
                            CurrentUser.Transactions.FindIndex(x => x.Id == transaction.Id)] = transaction;
                    }
                    catch    //if replace error just make new
                    {
                        MainViewModel.GetContext.CurrentUser.Transactions.Add(transaction);
                    }
                }
                else                //make new
                {
                    MainViewModel.GetContext.CurrentUser.Transactions.Add(transaction);
                }
                new Calculator().MakeTransaction(MainViewModel.GetContext.CurrentUser,
                        transaction.Spend);  //save here

                ListContext.NotifyTransactionList();

            }).Start();
        }


    }
}

using Economic_v2.Builders;
using Economic_v2.DataBaseLayer;
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
        object[] DataContexts = new object[3];         //here data context all controls where was Transaction
        UnitOfWork Data = UnitOfWorkSingleton.GetUnitOfWork;

        public TransactionsModel(object context)
        {
            DataContexts[0] = (new AddOrEditTransaction()).DataContext;
            DataContexts[1] = (new TransactionsListView()).DataContext;
            DataContexts[2] = context;
        }


        #region Contexts
        public AddOrEditTransactionsViewModel AddOrEditContext
        {
            get => (AddOrEditTransactionsViewModel)DataContexts[0];
        }

        public TransactionsListViewViewModel ListContext
        {
            get => (TransactionsListViewViewModel)DataContexts[1];
        }

        public TransactionsPageViewModel PageContext
        {
            get => (TransactionsPageViewModel)DataContexts[2];
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
                if (MainViewModel.CurrentUser.Transactions == null)  //if current not any note
                {
                    MainViewModel.CurrentUser.Transactions = new List<Transaction>();
                }
                Transaction category = AddOrEditContext.GetTransaction;  //convert to needed type
                if (isEdit)
                {
                    try         //if edit just replace
                    {
                        MainViewModel.CurrentUser.Transactions[MainViewModel.
                            CurrentUser.Transactions.FindIndex(x => x.Id == category.Id)] = category;
                    }
                    catch    //if replace error just make new
                    {
                        MainViewModel.CurrentUser.Transactions.Add(category);
                    }
                }
                else                //make new
                {
                    MainViewModel.CurrentUser.Transactions.Add(category);
                }
                Data.Users.Update(MainViewModel.CurrentUser);
                Data.Save();
            }).Start();
        }


    }
}

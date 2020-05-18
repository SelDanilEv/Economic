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
using Economic_v2.Pages;
using System.Threading.Tasks;

namespace Economic_v2.ViewModels
{
    public class TransactionsPageViewModel : ViewModelBase
    {
        public static TransactionsModel Model;

        public TransactionsPageViewModel()
        {
            IsEdit = false;
            Model = new TransactionsModel(this);
        }

        #region Controls
        private UserControl userControl;       //alterable usercontrol


        public UserControl TransactionPageControl        //handler alterable usercontrol
        {
            get
            {
                if (CreateFlag)               //set settings according to user control
                {
                    CreateOrConfirmTransactionCommand = new Command(OnConfirmCreateOrEditTransaction);
                    EditOrCancelTransactionCommand = new Command(OnCancelCreateOrEditTransaction);
                    userControl = Model.AddOrEditView;
                    _editTransactionButton = "Cancel";
                    _createTransactionButton = "Confirm";
                }
                else
                {
                    CreateOrConfirmTransactionCommand = new Command(OnCreateTransaction);
                    EditOrCancelTransactionCommand = new Command(OnEditTransaction);
                    userControl = Model.ListView;
                    _createTransactionButton = "Create Transaction";
                    _editTransactionButton = "Edit Transaction";
                }
                NotifyPropertyChanged("CreateConfirmTransactionButton");
                NotifyPropertyChanged("EditCancelTransactionButton");
                NotifyPropertyChanged("EditOrCancelTransaction");
                NotifyPropertyChanged("CreateOrConfirmTransaction");

                return userControl;           //change user control
            }
            set
            {
                userControl = value;
            }
        }
        #endregion

        #region View
        private bool _createFlag = false;
        public string DeleteButtonVisibility
        {
            get
            {
                if (_createFlag)
                    return "Hidden";
                else
                    return "Visible";
            }
        }

        public bool CreateFlag
        {
            get { return _createFlag; }
            set
            {
                _createFlag = value;
                NotifyPropertyChanged("DeleteButtonVisibility");
                NotifyPropertyChanged("IsEndableButtonsEditAndDelete");
                NotifyPropertyChanged("TransactionPageControl");
            }
        }

        private string _createTransactionButton;
        public string CreateConfirmTransactionButton
        {
            get
            {
                return _createTransactionButton;
            }
            set
            {
                _createTransactionButton = value;
            }
        }

        private string _editTransactionButton;
        public string EditCancelTransactionButton
        {
            get
            {
                return _editTransactionButton;
            }
            set
            {
                _editTransactionButton = value;
            }
        }

        private string _deleteTransactionButton = "Delete Transaction";
        public string DeleteTransactionButton
        {
            get => _deleteTransactionButton;
            set => _deleteTransactionButton = value;
        }


        public static Transaction SelectedTransaction
        {
            get
            {
                return TransactionsModel.ListContext.SelectedTransaction;
            }
        }

        public bool IsSelectedTransaction
        {
            get => TransactionsModel.ListContext.IsSelectedTransaction;
        }

        public bool IsEndableButtonsEditAndDelete
        {
            get
            {
                if (CreateFlag)
                    return true;
                if (IsSelectedTransaction)
                    return true;
                return false;
            }
        }

        public void NotifyEndableButtonsEditAndDelete()
        {
            NotifyPropertyChanged("IsEndableButtonsEditAndDelete");
        }


        public int DismissButtonProgress { get; set; }

        public string ShowDismissButton
        {
            get
            {
                if (DismissButtonProgress > 0 && DismissButtonProgress < 100)
                    return "Visible";
                return "Hidden";
            }
        }

        Task DeleteTransactionTask;
        public void RunDismissProgress()
        {
            DeleteTransactionTask = new Task(() =>
            {
                DismissButtonProgress = 0;
                while (DismissButtonProgress <= 100 && IsSelectedTransaction)
                {
                    DismissButtonProgress++;
                    Thread.Sleep(50);
                    NotifyPropertyChanged("DismissButtonProgress");
                    NotifyPropertyChanged("ShowDismissButton");
                }
                DismissButtonProgress = 0;
                if (!Model.DismissDelete)
                    Model.DeleteTransaction();
                else
                    Model.DeletedTransaction = null;
                NotifyPropertyChanged("DismissButtonProgress");
                NotifyPropertyChanged("ShowDismissButton");
            });
            DeleteTransactionTask.Start();
        }
        #endregion

        #region Commands
        private Command CreateOrConfirmTransactionCommand;
        public Command CreateOrConfirmTransaction
        {
            get
            {
                return CreateOrConfirmTransactionCommand;
            }
        }

        private Command EditOrCancelTransactionCommand;
        public Command EditOrCancelTransaction
        {
            get
            {
                return EditOrCancelTransactionCommand;
            }
        }

        private Command DeleteTransactionCommand;
        public Command DeleteTransaction
        {
            get
            {
                if (DeleteTransactionCommand == null)
                    DeleteTransactionCommand = new Command(OnDeleteTransaction);
                return DeleteTransactionCommand;
            }
        }

        private Command _dismissCommand;
        public Command DismissCommand
        {
            get
            {
                if (_dismissCommand == null)
                    _dismissCommand = new Command(OnDismiss);
                return _dismissCommand;
            }
        }


        #endregion

        #region ImplementationCommand
        private void ChangeFlag() => CreateFlag = !CreateFlag;    //change create flag

        private void OnCreateTransaction()           //just turn on create user control
        {
            new Task(() => TransactionsModel.AddOrEditContext.Initial()).Start();
            ChangeFlag();
        }

        private void OnConfirmCreateOrEditTransaction()           //confirm create or edit
        {
            if (TransactionsModel.AddOrEditContext.GetTransaction != null)
            {
                Model.ConfirmButton(IsEdit);
                IsEdit = false;
                ChangeFlag();
            }
        }

        public static bool IsEdit { get; set; }
        private void OnEditTransaction()             //if click edit
        {
            if (IsSelectedTransaction)
            {
                IsEdit = true;
                ChangeFlag();
                new Task(() => TransactionsModel.AddOrEditContext.Initial()).Start();
            }
        }

        private void OnCancelCreateOrEditTransaction()               //if click cancel
        {
            IsEdit = false;
            ChangeFlag();
            new Task(() => TransactionsModel.AddOrEditContext.Initial()).Start();
        }

        private void OnDeleteTransaction()
        {
            if (Model.DeletedTransaction != null)
            {
                if (SelectedTransaction.Id == Model.DeletedTransaction.Id)
                {
                    return;
                }
            }
            if (SelectedTransaction.Id == 0)
                return;
            if (IsSelectedTransaction)
            {
                if (DismissButtonProgress > 0)
                {
                    DismissButtonProgress = 101;
                    Task.WaitAll(DeleteTransactionTask);
                    Thread.Sleep(50);
                }

                Model.DeletedTransaction = SelectedTransaction;
                Model.DismissDelete = false;
                RunDismissProgress();
            }
        }

        private void OnDismiss()
        {
            Model.DismissDelete = true;
            DismissButtonProgress = 101;
        }
        #endregion

    }
}

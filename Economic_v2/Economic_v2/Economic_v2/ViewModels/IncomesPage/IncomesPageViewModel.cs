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
    public class IncomesPageViewModel : ViewModelBase
    {
        public static IncomesModel Model;

        public IncomesPageViewModel()
        {
            IsEdit = false;
            Model = new IncomesModel(this);
        }

        #region Controls
        private UserControl userControl;       //alterable usercontrol


        public UserControl IncomePageControl        //handler alterable usercontrol
        {
            get
            {
                if (CreateFlag)               //set settings according to user control
                {
                    CreateOrConfirmIncomeCommand = new Command(OnConfirmCreateOrEditIncome);
                    EditOrCancelIncomeCommand = new Command(OnCancelCreateOrEditIncome);
                    userControl = Model.AddOrEditView;
                    _editIncomeButton = "Cancel";
                    _createIncomeButton = "Confirm";
                }
                else
                {
                    CreateOrConfirmIncomeCommand = new Command(OnCreateIncome);
                    EditOrCancelIncomeCommand = new Command(OnEditIncome);
                    userControl = Model.ListView;
                    _createIncomeButton = "Create Income";
                    _editIncomeButton = "Edit Income";
                }
                NotifyPropertyChanged("CreateConfirmIncomeButton");
                NotifyPropertyChanged("EditCancelIncomeButton");
                NotifyPropertyChanged("EditOrCancelIncome");
                NotifyPropertyChanged("CreateOrConfirmIncome");

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
                NotifyPropertyChanged("IncomePageControl");
            }
        }

        private string _createIncomeButton;
        public string CreateConfirmIncomeButton
        {
            get
            {
                return _createIncomeButton;
            }
            set
            {
                _createIncomeButton = value;
            }
        }

        private string _editIncomeButton;
        public string EditCancelIncomeButton
        {
            get
            {
                return _editIncomeButton;
            }
            set
            {
                _editIncomeButton = value;
            }
        }

        private string _deleteIncomeButton = "Delete Income";
        public string DeleteIncomeButton
        {
            get => _deleteIncomeButton;
            set => _deleteIncomeButton = value;
        }


        public static Income SelectedIncome
        {
            get
            {
                return IncomesModel.ListContext.SelectedIncome;
            }
        }

        public bool IsSelectedIncome
        {
            get => IncomesModel.ListContext.IsSelectedIncome;
        }

        public bool IsEndableButtonsEditAndDelete
        {
            get
            {
                if (CreateFlag)
                    return true;
                if (IsSelectedIncome)
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

        Task DeleteIncomeTask;
        public void RunDismissProgress()
        {
            DeleteIncomeTask = new Task(() =>
            {
                DismissButtonProgress = 0;
                while (DismissButtonProgress <= 100 && IsSelectedIncome)
                {
                    DismissButtonProgress++;
                    Thread.Sleep(50);
                    NotifyPropertyChanged("DismissButtonProgress");
                    NotifyPropertyChanged("ShowDismissButton");
                }
                DismissButtonProgress = 0;
                if (!Model.DismissDelete)
                    Model.DeleteIncome();
                else
                    Model.DeletedIncome = null;
                NotifyPropertyChanged("DismissButtonProgress");
                NotifyPropertyChanged("ShowDismissButton");
            });
            DeleteIncomeTask.Start();
        }
        #endregion

        #region Commands
        private Command CreateOrConfirmIncomeCommand;
        public Command CreateOrConfirmIncome
        {
            get
            {
                return CreateOrConfirmIncomeCommand;
            }
        }

        private Command EditOrCancelIncomeCommand;
        public Command EditOrCancelIncome
        {
            get
            {
                return EditOrCancelIncomeCommand;
            }
        }

        private Command DeleteIncomeCommand;
        public Command DeleteIncome
        {
            get
            {
                if (DeleteIncomeCommand == null)
                    DeleteIncomeCommand = new Command(OnDeleteIncome);
                return DeleteIncomeCommand;
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

        private void OnCreateIncome()           //just turn on create user control
        {
            new Task(() => IncomesModel.AddOrEditContext.Initial()).Start();
            ChangeFlag();
        }

        private void OnConfirmCreateOrEditIncome()           //confirm create or edit
        {
            if (IncomesModel.AddOrEditContext.GetIncome != null)
            {
                Model.ConfirmButton(IsEdit);
                IsEdit = false;
                ChangeFlag();
            }
        }

        public static bool IsEdit { get; set; }
        private void OnEditIncome()             //if click edit
        {
            if (IsSelectedIncome)
            {
                IsEdit = true;
                ChangeFlag();
                new Task(() => IncomesModel.AddOrEditContext.Initial()).Start();
            }
        }

        private void OnCancelCreateOrEditIncome()               //if click cancel
        {
            IsEdit = false;
            ChangeFlag();
            new Task(() => IncomesModel.AddOrEditContext.Initial()).Start();
        }

        private void OnDeleteIncome()
        {
            if (Model.DeletedIncome != null)
            {
                if (SelectedIncome.Id == Model.DeletedIncome.Id)
                {
                    return;
                }
            }
            if (SelectedIncome.Id == 0)
                return;
            if (IsSelectedIncome)
            {
                if (DismissButtonProgress > 0)
                {
                    DismissButtonProgress = 101;
                    Task.WaitAll(DeleteIncomeTask);
                    Thread.Sleep(50);
                }

                Model.DeletedIncome = SelectedIncome;
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

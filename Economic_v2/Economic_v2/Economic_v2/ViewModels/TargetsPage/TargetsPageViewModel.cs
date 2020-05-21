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
    public class TargetsPageViewModel : ViewModelBase
    {
        public static TargetsModel Model;

        public TargetsPageViewModel()
        {
            IsEdit = false;
            Model = new TargetsModel(this);
        }

        #region Controls
        private UserControl userControl;       //alterable usercontrol


        public UserControl TargetPageControl        //handler alterable usercontrol
        {
            get
            {
                if (CreateFlag)               //set settings according to user control
                {
                    CreateOrConfirmTargetCommand = new Command(OnConfirmCreateOrEditTarget);
                    EditOrCancelTargetCommand = new Command(OnCancelCreateOrEditTarget);
                    userControl = Model.AddOrEditView;
                    _editTargetButton = "Cancel";
                    _createTargetButton = "Confirm";
                }
                else
                {
                    CreateOrConfirmTargetCommand = new Command(OnCreateTarget);
                    EditOrCancelTargetCommand = new Command(OnEditTarget);
                    userControl = Model.ListView;
                    _createTargetButton = "Create Goal";
                    _editTargetButton = "Edit Goal";
                }
                NotifyPropertyChanged("CreateConfirmTargetButton");
                NotifyPropertyChanged("EditCancelTargetButton");
                NotifyPropertyChanged("EditOrCancelTarget");
                NotifyPropertyChanged("CreateOrConfirmTarget");

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
                NotifyPropertyChanged("TargetPageControl");
            }
        }

        private string _createTargetButton;
        public string CreateConfirmTargetButton
        {
            get
            {
                return _createTargetButton;
            }
            set
            {
                _createTargetButton = value;
            }
        }

        private string _editTargetButton;
        public string EditCancelTargetButton
        {
            get
            {
                return _editTargetButton;
            }
            set
            {
                _editTargetButton = value;
            }
        }

        private string _deleteTargetButton = "Delete Goal";
        public string DeleteTargetButton
        {
            get => _deleteTargetButton;
            set => _deleteTargetButton = value;
        }


        public static Target SelectedTarget
        {
            get
            {
                return Model.ListContext.SelectedTarget;
            }
        }

        public bool IsSelectedTarget
        {
            get => Model.ListContext.IsSelectedTarget;
        }

        public bool IsEndableButtonsEditAndDelete
        {
            get
            {
                if (CreateFlag)
                    return true;
                if (IsSelectedTarget)
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

        Task DeleteTargetTask;
        public void RunDismissProgress()
        {
            DeleteTargetTask = new Task(() =>
            {
                DismissButtonProgress = 0;
                while (DismissButtonProgress <= 100 && IsSelectedTarget)
                {
                    DismissButtonProgress++;
                    Thread.Sleep(50);
                    NotifyPropertyChanged("DismissButtonProgress");
                    NotifyPropertyChanged("ShowDismissButton");
                }
                DismissButtonProgress = 0;
                if (!Model.DismissDelete)
                    Model.DeleteTarget();
                else
                    Model.DeletedTarget = null;
                NotifyPropertyChanged("DismissButtonProgress");
                NotifyPropertyChanged("ShowDismissButton");
            });
            DeleteTargetTask.Start();
        }
        #endregion

        #region Commands
        private Command CreateOrConfirmTargetCommand;
        public Command CreateOrConfirmTarget
        {
            get
            {
                return CreateOrConfirmTargetCommand;
            }
        }

        private Command EditOrCancelTargetCommand;
        public Command EditOrCancelTarget
        {
            get
            {
                return EditOrCancelTargetCommand;
            }
        }

        private Command DeleteTargetCommand;
        public Command DeleteTarget
        {
            get
            {
                if (DeleteTargetCommand == null)
                    DeleteTargetCommand = new Command(OnDeleteTarget);
                return DeleteTargetCommand;
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

        private void OnCreateTarget()           //just turn on create user control
        {
            new Task(() => Model.AddOrEditContext.Initial()).Start();
            ChangeFlag();
        }

        private void OnConfirmCreateOrEditTarget()           //confirm create or edit
        {
            if (Model.AddOrEditContext.GetTarget != null)
            {
                Model.ConfirmButton(IsEdit, Model.ListContext.Mode);
                IsEdit = false;
                ChangeFlag();
            }
        }

        public static bool IsEdit { get; set; }
        private void OnEditTarget()             //if click edit
        {
            if (IsSelectedTarget)
            {
                IsEdit = true;
                ChangeFlag();
                new Task(() => Model.AddOrEditContext.Initial()).Start();
            }
        }

        private void OnCancelCreateOrEditTarget()               //if click cancel
        {
            new Task(() => Model.AddOrEditContext.Initial()).Start();
            IsEdit = false;
            ChangeFlag();
        }

        private void OnDeleteTarget()
        {
            if (Model.DeletedTarget != null)
            {
                if (SelectedTarget.Id == Model.DeletedTarget.Id)
                {
                    return;
                }
            }
            if (SelectedTarget != null)
            {
                if (SelectedTarget.Id == 0)
                    return;
            }
            else { return; }
            if (IsSelectedTarget)
            {
                if (DismissButtonProgress > 0)
                {
                    DismissButtonProgress = 101;
                    Task.WaitAll(DeleteTargetTask);
                    Thread.Sleep(50);
                }

                Model.deleteMode = Model.ListContext.Mode;
                Model.DeletedTarget = SelectedTarget;
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

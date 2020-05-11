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
    public class CategoriesPageViewModel : ViewModelBase
    {
        public static CategoriesModel Model;

        public CategoriesPageViewModel()
        {
            IsEdit = false;
            Model = new CategoriesModel(this);
        }

        #region Controls
        private UserControl userControl = new CategoriesListView();       //alterable usercontrol


        public UserControl CategoryPageControl        //handler alterable usercontrol
        {
            get
            {
                if (CreateFlag)               //set settings according to user control
                {
                    CreateOrConfirmCategoryCommand = new Command(OnConfirmCreateOrEditCategory);
                    EditOrCancelCategoryCommand = new Command(OnCancelCreateOrEditCategory);
                    userControl = new AddOrEditCategory();
                    _editCategoryButton = "Cancel";
                    _createCategoryButton = "Confirm";
                }
                else
                {
                    CreateOrConfirmCategoryCommand = new Command(OnCreateCategory);
                    EditOrCancelCategoryCommand = new Command(OnEditCategory);
                    userControl = new CategoriesListView();
                    _createCategoryButton = "Create Category";
                    _editCategoryButton = "Edit Category";
                }
                NotifyPropertyChanged("CreateConfirmCategoryButton");
                NotifyPropertyChanged("EditCancelCategoryButton");
                NotifyPropertyChanged("EditOrCancelCategory");
                NotifyPropertyChanged("CreateOrConfirmCategory");

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
                NotifyPropertyChanged("CategoryPageControl");
            }
        }

        private string _createCategoryButton;
        public string CreateConfirmCategoryButton
        {
            get
            {
                return _createCategoryButton;
            }
            set
            {
                _createCategoryButton = value;
            }
        }

        private string _editCategoryButton;
        public string EditCancelCategoryButton
        {
            get
            {
                return _editCategoryButton;
            }
            set
            {
                _editCategoryButton = value;
            }
        }

        private string _deleteCategoryButton = "Delete Category";
        public string DeleteCategoryButton
        {
            get => _deleteCategoryButton;
            set => _deleteCategoryButton = value;
        }


        public static Category SelectedCategory
        {
            get
            {
                return Model.ListContext.SelectedCategory;
            }
        }

        public bool IsSelectedCategory
        {
            get => Model.ListContext.IsSelectedCategory;
        }

        public bool IsEndableButtonsEditAndDelete
        {
            get
            {
                if (CreateFlag)
                    return true;
                if (IsSelectedCategory)
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

        Task DeleteCategoryTask;
        public void RunDismissProgress()
        {
            DeleteCategoryTask = new Task(() =>
            {
                DismissButtonProgress = 0;
                while (DismissButtonProgress <= 100 && IsSelectedCategory)
                {
                    DismissButtonProgress++;
                    Thread.Sleep(50);
                    NotifyPropertyChanged("DismissButtonProgress");
                    NotifyPropertyChanged("ShowDismissButton");
                }
                DismissButtonProgress = 0;
                if (!Model.DismissDelete)
                    Model.DeleteCategory();
                else
                    Model.DeletedCategory = null;
                NotifyPropertyChanged("DismissButtonProgress");
                NotifyPropertyChanged("ShowDismissButton");
            });
            DeleteCategoryTask.Start();
        }
        #endregion

        #region Commands
        private Command CreateOrConfirmCategoryCommand;
        public Command CreateOrConfirmCategory
        {
            get
            {
                return CreateOrConfirmCategoryCommand;
            }
        }

        private Command EditOrCancelCategoryCommand;
        public Command EditOrCancelCategory
        {
            get
            {
                return EditOrCancelCategoryCommand;
            }
        }

        private Command DeleteCategoryCommand;
        public Command DeleteCategory
        {
            get
            {
                if (DeleteCategoryCommand == null)
                    DeleteCategoryCommand = new Command(OnDeleteCategory);
                return DeleteCategoryCommand;
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

        private void OnCreateCategory()           //just turn on create user control
        {
            ChangeFlag();
        }

        private void OnConfirmCreateOrEditCategory()           //confirm create or edit
        {
            if (Model.AddOrEditContext.GetCategory != null)
            {
                Model.ConfirmButton(IsEdit);
                IsEdit = false;
                ChangeFlag();
            }
        }

        public static bool IsEdit { get; set; }
        private void OnEditCategory()             //if click edit
        {
            if (IsSelectedCategory)
            {
                Model.AddOrEditContext.ClearFields = true;
                IsEdit = true;
                ChangeFlag();
            }
        }

        private void OnCancelCreateOrEditCategory()               //if click cancel
        {
            Model.AddOrEditContext.ClearFields = true;
            IsEdit = false;
            ChangeFlag();
        }

        private void OnDeleteCategory()
        {
            if (Model.DeletedCategory != null)
            {
                if (SelectedCategory.Id == Model.DeletedCategory.Id)
                {
                    return;
                }
            }
            if (IsSelectedCategory)
            {
                if (DismissButtonProgress > 0)
                {
                    DismissButtonProgress = 101;
                    Task.WaitAll(DeleteCategoryTask);
                    Thread.Sleep(50);
                }

                Model.DeletedCategory = SelectedCategory;
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

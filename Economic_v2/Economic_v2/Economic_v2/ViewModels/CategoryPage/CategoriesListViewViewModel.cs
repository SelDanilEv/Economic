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
    public class CategoriesListViewViewModel : ViewModelBase
    {
        public CategoriesListViewViewModel()
        {

        }

        public void ReadCategories()   // get actual notes
        {
            if (MainViewModel.CurrentUser.Categories == null)
                MainViewModel.CurrentUser.Categories = new List<Category>();
            List<Category> UserCategories = new List<Category>( MainViewModel.CurrentUser.Categories);
           
            while (UserCategories.Count < 7)     //make empty notes to make minimem 10
            {
                UserCategories.Add(new Category("Empty",0));
            }
            Categories = UserCategories;
        }

        #region View
        public ObservableCollection<Category> CategoriesObs
        {
            get
            {
                return new ObservableCollection<Category>(Categories);
            }
        }

        private List<Category> _categories;

        public List<Category> Categories
        {
            get { ReadCategories(); return _categories; }
            set { _categories = value; }
        }


        private Category _selectedCategory { get; set; }
        public Category SelectedCategory
        {
            get
            {
                return _selectedCategory;
            }
            set
            {
                _selectedCategory = value;
                CategoriesPageViewModel.Model.NotifyEndableButtonsEditAndDelete();
            }
        }

        public bool IsSelectedCategory
        {
            get
            {
                if (_selectedCategory == null)
                    return false;
                return true;
            }
        }
        #endregion

        #region ImplementationCommand
        public void OnDeleteCategory(Category target)             //call via context
        {
            MainViewModel.CurrentUser.Categories.RemoveAll(
                x => x.Id == target.Id);
            UnitOfWorkSingleton.GetUnitOfWork.Users.Update(MainViewModel.CurrentUser);
            UnitOfWorkSingleton.GetUnitOfWork.Save();
            NotifyPropertyChanged("CategoriesObs");
        }
        #endregion
    }
}

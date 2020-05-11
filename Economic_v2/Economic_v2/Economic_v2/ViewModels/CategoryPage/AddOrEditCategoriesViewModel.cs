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
    public class AddOrEditCategoriesViewModel : ViewModelBase
    {
        public AddOrEditCategoriesViewModel()
        {
            Initial();
        }

        #region Setup
        public void Initial()
        {
            if (!CategoriesPageViewModel.IsEdit)   //make start settings
            {
                ReturnedCategory = new Category();
                IfCreate();
            }
            else
            {
                ReturnedCategory = new Category(CategoriesPageViewModel.SelectedCategory);

                if (ReturnedCategory.Id == 0)
                {
                    IfCreate();
                }
                else
                {
                    IfEdit();
                }
            }

            NotifyPropertyChanged("CategoryNameError");
            NotifyPropertyChanged("SpendError");
            ClearFields = false;
        }

        public void IfCreate()
        {
            _categoryName = "";
            spendvalue = null;

             _spend = Double.PositiveInfinity;
            SpendError = CategoryNameError= "Requared value";

        }

        public void IfEdit()
        {
            _categoryName = ReturnedCategory.CategoryName;
            _spend = ReturnedCategory.Spend;

            SpendError = CategoryNameError = null;
        }
        #endregion

        #region View
        public bool ClearFields = false;

        private string _categoryName;
        public string CategoryNameError { get; set; }
        public string CategoryName
        {
            get
            {
                if (ClearFields)       //if need clear
                    new Task(() => Initial()).Start();
                return _categoryName;
            }
            set
            {
                _categoryName = value;
                validateCategoryName();
                NotifyPropertyChanged("CategoryNameError");
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

        #region GetCategory
        public bool SuccessfulValidation => validateAll();

        private bool validateCategoryName()
        {
            if (_categoryName.Length < 2)
            {
                CategoryNameError = "Category name too short";
                return false;
            }

            if (_categoryName[0] == ' ')
            {
                CategoryNameError = "Category name musn't started with space";
                return false;
            }

            ReturnedCategory.CategoryName = _categoryName;
            CategoryNameError = null;
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
                SpendError = "Monthly expenses must be positive";
                return false;
            }

            if (_spend == Double.PositiveInfinity)
            {
                SpendError = "Requared value";
                return false;
            }

            ReturnedCategory.Spend = _spend;
            SpendError = null;
            return true;
        }

        public bool validateAll()
        {
            bool rv = validateCategoryName() &&
            validateSpend() ;

            NotifyPropertyChanged("CategoryNameError");
            NotifyPropertyChanged("SpendError");
            return rv;
        }

        public Category ReturnedCategory;

        public Category GetCategory
        {
            get
            {
                if (SuccessfulValidation)
                {
                    ClearFields = true;
                    return ReturnedCategory;
                }
                return null;
            }
        }
        #endregion

    }
}

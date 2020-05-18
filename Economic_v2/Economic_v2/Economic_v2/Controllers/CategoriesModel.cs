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
    public class CategoriesModel
    {
        static object[] DataContexts = new object[3];         //here data context all controls where was Category
        object[] Views = new object[3];         //here Views
        UnitOfWork Data = UnitOfWorkSingleton.GetUnitOfWork;

        public CategoriesModel(object context)
        {
            Views[0] = new AddOrEditCategory();
            Views[1] = new CategoriesListView();
            DataContexts[0] = ((AddOrEditCategory)Views[0]).DataContext;
            DataContexts[1] = ((CategoriesListView)Views[1]).DataContext;
            DataContexts[2] = context;
        }



        #region Contexts
        public static AddOrEditCategoriesViewModel AddOrEditContext
        {
            get => (AddOrEditCategoriesViewModel)DataContexts[0];
        }

        public static CategoriesListViewViewModel ListContext
        {
            get => (CategoriesListViewViewModel)DataContexts[1];
        }

        public static CategoriesPageViewModel PageContext
        {
            get => (CategoriesPageViewModel)DataContexts[2];
        }
        #endregion

        #region Views
        public AddOrEditCategory AddOrEditView
        {
            get => (AddOrEditCategory)Views[0];
        }

        public CategoriesListView ListView
        {
            get => (CategoriesListView)Views[1];
        }

        public CategoriesPage PageView
        {
            get => (CategoriesPage)Views[2];
        }
        #endregion

        public void NotifyEndableButtonsEditAndDelete()
        {
            PageContext.NotifyEndableButtonsEditAndDelete();
        }

        public bool DismissDelete;
        public Category DeletedCategory;
        public void DeleteCategory()
        {
            new Task(() =>    //start method in context
            {
                ListContext.OnDeleteCategory(new Category(DeletedCategory));
                DeletedCategory = null;
            }).Start();
        }

        public void ConfirmButton(bool isEdit)
        {
            new Task(() =>
            {
                if (MainViewModel.GetContext.CurrentUser.Categories == null)  //if current not any note
                {
                    MainViewModel.GetContext.CurrentUser.Categories = new List<Category>();
                }
                Category category = AddOrEditContext.GetCategory;
                if (isEdit)
                {
                    try         //if edit just replace
                    {
                        MainViewModel.GetContext.CurrentUser.Categories[MainViewModel.GetContext.
                            CurrentUser.Categories.FindIndex(x => x.Id == category.Id)] = category;
                    }
                    catch    //if replace error just make new
                    {
                        MainViewModel.GetContext.CurrentUser.Categories.Add(category);
                    }
                }
                else                //make new
                {
                    MainViewModel.GetContext.CurrentUser.Categories.Add(category);
                }

                ListContext.NotifyCategoryList();

                if (StatisticViewModel.GetContext != null)
                    StatisticViewModel.GetContext.MakeCalculate();

                Data.Users.Update(MainViewModel.GetContext.CurrentUser);
                Data.Save();
            }).Start();
        }


    }
}

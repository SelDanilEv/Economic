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
        object[] DataContexts = new object[3];         //here data context all controls where was Category
        UnitOfWork Data = UnitOfWorkSingleton.GetUnitOfWork;

        public CategoriesModel(object context)
        {
            DataContexts[0] = (new AddOrEditCategory()).DataContext;
            DataContexts[1] = (new CategoriesListView()).DataContext;
            DataContexts[2] = context;
        }


        #region Contexts
        public AddOrEditCategoriesViewModel AddOrEditContext
        {
            get => (AddOrEditCategoriesViewModel)DataContexts[0];
        }

        public CategoriesListViewViewModel ListContext
        {
            get => (CategoriesListViewViewModel)DataContexts[1];
        }

        public CategoriesPageViewModel PageContext
        {
            get => (CategoriesPageViewModel)DataContexts[2];
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
                if (MainViewModel.CurrentUser.Categories == null)  //if current not any note
                {
                    MainViewModel.CurrentUser.Categories = new List<Category>();
                }
                Category category = AddOrEditContext.GetCategory;  //convert to needed type
                if (isEdit)
                {
                    try         //if edit just replace
                    {
                        MainViewModel.CurrentUser.Categories[MainViewModel.
                            CurrentUser.Categories.FindIndex(x => x.Id == category.Id)] = category;
                    }
                    catch    //if replace error just make new
                    {
                        MainViewModel.CurrentUser.Categories.Add(category);
                    }
                }
                else                //make new
                {
                    MainViewModel.CurrentUser.Categories.Add(category);
                }
                Data.Users.Update(MainViewModel.CurrentUser);
                Data.Save();
            }).Start();
        }


    }
}

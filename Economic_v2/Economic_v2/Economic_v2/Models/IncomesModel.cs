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
    public class IncomesModel
    {
        object[] DataContexts = new object[3];         //here data context all controls where was Income
        UnitOfWork Data = UnitOfWorkSingleton.GetUnitOfWork;

        public IncomesModel(object context)
        {
            DataContexts[0] = (new AddOrEditIncome()).DataContext;
            DataContexts[1] = (new IncomesListView()).DataContext;
            DataContexts[2] = context;
        }


        #region Contexts
        public AddOrEditIncomesViewModel AddOrEditContext
        {
            get => (AddOrEditIncomesViewModel)DataContexts[0];
        }

        public IncomesListViewViewModel ListContext
        {
            get => (IncomesListViewViewModel)DataContexts[1];
        }

        public IncomesPageViewModel PageContext
        {
            get => (IncomesPageViewModel)DataContexts[2];
        }
        #endregion

        public void NotifyEndableButtonsEditAndDelete()
        {
            PageContext.NotifyEndableButtonsEditAndDelete();
        }

        public bool DismissDelete;
        public Income DeletedIncome;
        public void DeleteIncome()
        {
            new Task(() =>    //start method in context
            {
                ListContext.OnDeleteIncome(new Income(DeletedIncome));
                DeletedIncome = null;
            }).Start();
        }

        public void ConfirmButton(bool isEdit)
        {
            new Task(() =>
            {
                if (MainViewModel.CurrentUser.Incomes == null)  //if current not any note
                {
                    MainViewModel.CurrentUser.Incomes = new List<Income>();
                }
                Income category = AddOrEditContext.GetIncome;  //convert to needed type
                if (isEdit)
                {
                    try         //if edit just replace
                    {
                        MainViewModel.CurrentUser.Incomes[MainViewModel.
                            CurrentUser.Incomes.FindIndex(x => x.Id == category.Id)] = category;
                    }
                    catch    //if replace error just make new
                    {
                        MainViewModel.CurrentUser.Incomes.Add(category);
                    }
                }
                else                //make new
                {
                    MainViewModel.CurrentUser.Incomes.Add(category);
                }
                Data.Users.Update(MainViewModel.CurrentUser);
                Data.Save();
            }).Start();
        }


    }
}

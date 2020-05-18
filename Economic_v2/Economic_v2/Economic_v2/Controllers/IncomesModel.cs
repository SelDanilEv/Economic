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
        static object[] DataContexts = new object[3];         //here data context all controls where was Income
        object[] Views = new object[3];         //here Views
        UnitOfWork Data = UnitOfWorkSingleton.GetUnitOfWork;

        public IncomesModel(object context)
        {
            Views[0] = new AddOrEditIncome();
            Views[1] = new IncomesListView();
            DataContexts[0] = ((AddOrEditIncome)Views[0]).DataContext;
            DataContexts[1] = ((IncomesListView)Views[1]).DataContext;
            DataContexts[2] = context;
        }


        #region Contexts
        public static AddOrEditIncomesViewModel AddOrEditContext
        {
            get => (AddOrEditIncomesViewModel)DataContexts[0];
        }

        public static IncomesListViewViewModel ListContext
        {
            get => (IncomesListViewViewModel)DataContexts[1];
        }

        public static IncomesPageViewModel PageContext
        {
            get => (IncomesPageViewModel)DataContexts[2];
        }
        #endregion

        #region Views
        public AddOrEditIncome AddOrEditView
        {
            get => (AddOrEditIncome)Views[0];
        }

        public IncomesListView ListView
        {
            get => (IncomesListView)Views[1];
        }

        public IncomesPage PageView
        {
            get => (IncomesPage)Views[2];
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
                if (MainViewModel.GetContext.CurrentUser.Incomes == null)  //if current not any note
                {
                    MainViewModel.GetContext.CurrentUser.Incomes = new List<Income>();
                }
                Income income = AddOrEditContext.GetIncome;
                if (isEdit)
                {
                    try         //if edit just replace
                    {
                        MainViewModel.GetContext.CurrentUser.Incomes[MainViewModel.GetContext.
                            CurrentUser.Incomes.FindIndex(x => x.Id == income.Id)] = income;
                    }
                    catch    //if replace error just make new
                    {
                        MainViewModel.GetContext.CurrentUser.Incomes.Add(income);
                    }
                }
                else                //make new
                {
                    MainViewModel.GetContext.CurrentUser.Incomes.Add(income);
                }

                ListContext.NotifyIncomeList();

                if (StatisticViewModel.GetContext != null)
                    StatisticViewModel.GetContext.MakeCalculate();

                Data.Users.Update(MainViewModel.GetContext.CurrentUser);
                Data.Save();
            }).Start();
        }


    }
}

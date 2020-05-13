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
    public class IncomesListViewViewModel : ViewModelBase
    {
        public IncomesListViewViewModel()
        {

        }

        public void ReadIncomes()   // get actual notes
        {
            if (MainViewModel.CurrentUser.Incomes == null)
                MainViewModel.CurrentUser.Incomes = new List<Income>();
            List<Income> UserIncomes = new List<Income>( MainViewModel.CurrentUser.Incomes);
           
            while (UserIncomes.Count < 7)     //make empty notes to make minimem 10
            {
                UserIncomes.Add(new Income("Empty",0,DateTime.Now.AddDays(1)));
            }
            Incomes = UserIncomes;
        }

        #region View
        public ObservableCollection<Income> IncomesObs
        {
            get
            {
                return new ObservableCollection<Income>(Incomes);
            }
        }

        private List<Income> _incomes;

        public List<Income> Incomes
        {
            get { ReadIncomes(); return _incomes; }
            set { _incomes = value; }
        }


        private Income _selectedIncome { get; set; }
        public Income SelectedIncome
        {
            get
            {
                return _selectedIncome;
            }
            set
            {
                _selectedIncome = value;
                IncomesPageViewModel.Model.NotifyEndableButtonsEditAndDelete();
            }
        }

        public bool IsSelectedIncome
        {
            get
            {
                if (_selectedIncome == null)
                    return false;
                return true;
            }
        }
        #endregion

        #region ImplementationCommand
        public void OnDeleteIncome(Income target)             //call via context
        {
            MainViewModel.CurrentUser.Incomes.RemoveAll(
                x => x.Id == target.Id);
            UnitOfWorkSingleton.GetUnitOfWork.Users.Update(MainViewModel.CurrentUser);
            UnitOfWorkSingleton.GetUnitOfWork.Save();
            NotifyPropertyChanged("IncomesObs");
        }
        #endregion
    }
}

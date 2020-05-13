using Economic_v2.Builders;
using Economic_v2.Commands;
using Economic_v2.DataBaseLayer;
using Economic_v2.Models;
using Economic_v2.Pages;
using Economic_v2.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Economic_v2.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private static object locker = new Object();

        private UnitOfWork unitOfWork;

        public MainViewModel()
        {
            unitOfWork = UnitOfWorkSingleton.GetUnitOfWork;
        }


        private static User _currentUser;
        public static User CurrentUser        //Your User
        {
            get { lock (locker) { return _currentUser; } }
            set { lock (locker) { _currentUser = value; } }
        }


        public static Window loginWindow;          //references on windows
        public static Window mainWindow;

        #region View
        private UserControl _mainUserControl = new HomePage();
        public UserControl MainUserControl
        {
            get => _mainUserControl;
            set => _mainUserControl = value;
        }

        private int _selectedPageIndex;
        public int SelectedPageIndex
        {
            get
            {
                return _selectedPageIndex;
            }
            set
            {
                _selectedPageIndex = value;
                switch (_selectedPageIndex)
                {
                    case 0:
                        MainUserControl = new HomePage();
                        break;
                    case 1:
                        MainUserControl = new TargetsPage();
                        break;
                    case 2:
                        MainUserControl = new CategoriesPage();
                        break;
                    case 3:
                        MainUserControl = new IncomesPage();
                        break;
                    case 4:
                        MainUserControl = new TransactionsPage();
                        break;
                    case 5:
                        MainUserControl = new StatisticPage();
                        break;
                }
                NotifyPropertyChanged("CursorPosition");
                NotifyPropertyChanged("MainUserControl");
            }
        }

        public Thickness CursorPosition
        {
            get => new Thickness(0, (90 + (60 * _selectedPageIndex)), 0, 0);
        }
        #endregion


        #region Commands
        private Command CloseAppCommand;            //close all windows
        public Command CloseApp
        {
            get
            {
                if (this.CloseAppCommand == null)
                {
                    CloseAppCommand = new Command(OnCloseApp);
                }
                return CloseAppCommand;
            }
        }

        private Command _showSettingsCommand;            
        public Command ShowSettings
        {
            get
            {
                if (this._showSettingsCommand == null)
                {
                    _showSettingsCommand = new Command(OnShowSettings);
                }
                return _showSettingsCommand;
            }
        }


        #endregion

        #region ImplementationCommand
        private void OnCloseApp()
        {
            unitOfWork.Save();
            mainWindow.Close();
            loginWindow.Close();
        }

        private void OnShowSettings()
        {
            _selectedPageIndex = -1;
            MainUserControl = new SettingsPage();
            NotifyPropertyChanged("MainUserControl");
            NotifyPropertyChanged("SelectedPageIndex");
        }
        #endregion
    }
}

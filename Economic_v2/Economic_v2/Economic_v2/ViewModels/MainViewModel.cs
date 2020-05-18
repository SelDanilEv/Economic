using Economic_v2.Builders;
using Economic_v2.Commands;
using Economic_v2.DataBaseLayer;
using Economic_v2.Logic;
using Economic_v2.Models;
using Economic_v2.Pages;
using Economic_v2.Services;
using Economic_v2.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Economic_v2.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private static object locker = new Object();
        private static object _context;
        public object[] _pages = new object[10];

        private UnitOfWork unitOfWork;

        public MainViewModel()
        {
            _context = this;
            unitOfWork = UnitOfWorkSingleton.GetUnitOfWork;
            AttentionVisible = "Hidden";
        }

        public static MainViewModel GetContext
        {
            get => (MainViewModel)_context;
        }

        public static Window loginWindow;          //references on windows
        public static Window mainWindow;

        private User _currentUser;
        public User CurrentUser        //Your User
        {
            get { lock (locker) { return _currentUser; } }
            set { lock (locker) { _currentUser = value; } }
        }

        #region Update

        public void UpdateInfo()
        {
            (new Task(() =>
            {
                if (TargetsPageViewModel.Model != null)
                    TargetsPageViewModel.Model.ListContext.NotifyTargetList();
                if (StatisticViewModel.GetContext != null)
                    StatisticViewModel.GetContext.MakeCalculate();
                if (SettingPageViewModel.GetContext != null)
                    SettingPageViewModel.GetContext.Setup();
                if (CategoriesModel.ListContext != null)
                    CategoriesModel.ListContext.NotifyCategoryList();
                if (IncomesModel.ListContext != null)
                    IncomesModel.ListContext.NotifyIncomeList();
                if (TransactionsModel.ListContext != null)
                    TransactionsModel.ListContext.NotifyTransactionList();
                if (HomeViewModel.GetContext != null)
                {
                    HomeViewModel.GetContext.NotifyTargetList();
                    HomeViewModel.GetContext.FindLargestTarget();
                }
                NotifyPropertyChanged("Free_TotalMoney");
                NotifyPropertyChanged("UserName");
                NotifyPropertyChanged("MoneyColor");
                NotifyPropertyChanged("AttentionVisible");
            })).Start();
        }
        #endregion

        #region View
        public string UserName
        {
            get
            {
                if (_currentUser != null)
                {
                    if (_currentUser.Login.Length > 11)
                        return _currentUser.Login.Substring(0, 11);
                    return _currentUser.Login;
                }
                return "";
            }
        }

        public SolidColorBrush MoneyColor { get; set; }

        public string AttentionVisible { get; set; }

        public string Free_TotalMoney
        {
            get
            {
                string outstr = "";
                if (_currentUser != null)
                {
                    outstr = (_currentUser.Total_money - _currentUser.Reserve_money).ToString() + '/' + _currentUser.Total_money.ToString();
                    Color color;
                    if (_currentUser.Total_money - _currentUser.Reserve_money < 0)
                    {
                        color = (Color)ColorConverter.ConvertFromString("#8B0000");
                        AttentionVisible = "Visible";
                    }
                    else
                    {
                        color = (Color)ColorConverter.ConvertFromString("#FF0069C0");
                        AttentionVisible = "Hidden";
                    }
                    MoneyColor = new SolidColorBrush(color);
                    NotifyPropertyChanged("MoneyColor");
                }
                return outstr;
            }
        }

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
                        MainUserControl = (HomePage)_pages[0];
                        break;
                    case 1:
                        MainUserControl = (TargetsPage)_pages[1];
                        break;
                    case 2:
                        MainUserControl = (CategoriesPage)_pages[2];
                        break;
                    case 3:
                        MainUserControl = (IncomesPage)_pages[3];
                        break;
                    case 4:
                        MainUserControl = (TransactionsPage)_pages[4];
                        break;
                    case 5:
                        MainUserControl = (StatisticPage)_pages[5];
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


        private bool feature
        {
            get
            {
                if (CurrentUser != null)
                    return CurrentUser.Statistic.MoodChange % 12 == 0;
                return false;
            }
        }
        public string FeatureVis
        {
            get
            {
                if (feature)
                    return "Visible";
                return "Hidden";
            }
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

        private Command _showNoteCommand;
        public Command ShowNote
        {
            get
            {
                if (_showNoteCommand == null)
                {
                    _showNoteCommand = new Command(OnNoteSettings);
                }
                return _showNoteCommand;
            }
        }

        private Command _makeCalculateAndUpdate;
        public Command MakeCalculateAndUpdate
        {
            get
            {
                if (this._makeCalculateAndUpdate == null)
                {
                    _makeCalculateAndUpdate = new Command(OnUpdateMainWindow);
                }
                return _makeCalculateAndUpdate;
            }
        }

        private Command _changeMood;
        public Command ChangeMood
        {
            get
            {
                if (_changeMood == null)
                {
                    _changeMood = new Command(OnChangeMood);
                }
                return _changeMood;
            }
        }
        #endregion

        #region ImplementationCommand
        private void OnCloseApp()
        {
            mainWindow.Close();
            loginWindow.Close();
        }

        private void OnShowSettings()
        {
            _selectedPageIndex = -1;
            MainUserControl = (SettingsPage)_pages[6];
            NotifyPropertyChanged("MainUserControl");
            NotifyPropertyChanged("SelectedPageIndex");
        }

        private void OnNoteSettings()
        {
            _selectedPageIndex = -1;
            SettingPageViewModel.GetContext.Setup();
            MainUserControl = (NotePage)_pages[7];
            NotifyPropertyChanged("MainUserControl");
            NotifyPropertyChanged("SelectedPageIndex");
        }

        public void OnUpdateMainWindow()
        {
            (new Task(() => new Calculator().Calculate(_currentUser))).Start();
        }

        public void OnChangeMood()
        {
            CurrentUser.Statistic.MoodChange++;
            NotifyPropertyChanged("FeatureVis");
        }
        #endregion

    }
}

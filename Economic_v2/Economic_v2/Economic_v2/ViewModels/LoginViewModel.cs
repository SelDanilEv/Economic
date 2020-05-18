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
using System.Threading.Tasks;
using Economic_v2.PasswordCoder;
using Economic_v2.Pages;
using Economic_v2.Services;
using System.Windows.Controls;

namespace Economic_v2.ViewModels
{
    class LoginViewModel : ViewModelBase
    {
        private UnitOfWork unitOfWork;   // pattern for work with database

        public static Window MainWindow;   //reference on windows for working
        public static Window LoginWindow;

        private static object _context;

        public LoginViewModel()
        {
            unitOfWork = UnitOfWorkSingleton.GetUnitOfWork;
            PageMode = Mode.LogIn;
            _context = this;
            LoadVis = "Hidden";
        }

        public static LoginViewModel GetContext
        {
            get => (LoginViewModel)_context;
        }

        #region View   
        enum Mode
        {
            LogIn = 0,
            Registrate,
            ReestablishPassword
        }

        Mode _pageMode;
        Mode PageMode
        {
            get
            {
                return _pageMode;
            }
            set  //all alterble logic
            {
                _pageMode = value;

                switch (_pageMode)
                {
                    case Mode.LogIn:
                        HeaderText = "Welcome";
                        ConfirmButtonText = "Log in";
                        Registrate_LoginButtonText = "Registration";
                        ConfirmButton = new RelayCommand<Window>(OnLogin);
                        Registrate_LoginButton = new Command(ChangeOnRegistration);
                        ReestablishPassword = new Command(ChangeOnReestablishPassword);
                        break;
                    case Mode.Registrate:
                        HeaderText = "Registration";
                        ConfirmButtonText = "Registrate";
                        Registrate_LoginButtonText = "Log in";
                        ConfirmButton = new RelayCommand<Window>(OnRegistrate);
                        Registrate_LoginButton = new Command(ChangeOnLogIn);
                        ReestablishPassword = new Command(ChangeOnReestablishPassword);
                        break;
                    case Mode.ReestablishPassword:
                        HeaderText = "Recovery";
                        ConfirmButtonText = "Reestablish";
                        Registrate_LoginButtonText = "Log in";
                        ConfirmButton = new RelayCommand<Window>(OnReestablish);
                        Registrate_LoginButton = new Command(ChangeOnLogIn);
                        ReestablishPassword = new Command(ChangeOnLogIn);
                        break;
                }

                NotifyPropertyChanged("HeaderText");
                NotifyPropertyChanged("ConfirmButtonText");
                NotifyPropertyChanged("Registrate_LoginButtonText");
                NotifyPropertyChanged("ConfirmButton");
                NotifyPropertyChanged("Registrate_LoginButton");
                NotifyPropertyChanged("ReestablishPassword");
            }
        }

        public string HeaderText { get; set; }

        public string ConfirmButtonText { get; set; }
        public string Registrate_LoginButtonText { get; set; }

        private string _login;
        private string _password;

        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value;
                if (validateLogin())
                {
                    GetUserByLogin(value);
                }
                NotifyPropertyChanged("LoginError");
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                validatePassword();
                NotifyPropertyChanged("PasswordError");
            }
        }

        public string LoginError { get; set; }
        public string PasswordError { get; set; }

        public string LoadVis { get; set; }


        public void ClearFields()
        {
            _login = _password = PasswordError = LoginError = null;

            NotifyPropertyChanged("Login");
            NotifyPropertyChanged("Password");
            NotifyPropertyChanged("PasswordError");
            NotifyPropertyChanged("LoginError");
        }
        #endregion

        #region Validation
        private bool validateLogin()
        {
            if (_login == "")
            {
                LoginError = "Login is empty";
                return false;
            }

            if (_login.Length < 2)
            {
                LoginError = "Login too short";
                return false;
            }
            if (_login[0] == ' ')
            {
                LoginError = "Login mustn't starts with space";
                return false;
            }

            LoginError = null;
            return true;
        }

        private bool validatePassword()
        {
            if (_password == "")
            {
                PasswordError = "Password is empty";
                return false;
            }

            if (_password.Length < 6)
            {
                PasswordError = "Password too short";
                return false;
            }
            PasswordError = null;
            return true;
        }


        #endregion

        #region Get User region
        private User TargetUser = null;
        private void GetUserByLogin(string login)
        {
            (new Task(() =>
            {
                if (CheckConnectionTask.Status == TaskStatus.Running)
                {
                    time = 200;
                    Thread.Sleep(100);
                }
                time = 0;
                TargetUser = null;
                connectionChecker();
                TargetUser = UnitOfWorkSingleton.GetUnitOfWork.Users.GetPure(login);
            })).Start();
        }

        private int time;
        private Task CheckConnectionTask = new Task(() => { });
        private void connectionChecker()
        {
            CheckConnectionTask = new Task(() =>
            {
                LoadVis = "Visible";
                NotifyPropertyChanged("LoadVis");

                while (time < 50 && TargetUser == null)
                {
                    Thread.Sleep(100);
                    time++;
                }
                LoadVis = "Hidden";
                NotifyPropertyChanged("LoadVis");

                switch (TargetUser.Id)
                {
                    case 0:
                        LoginError = "Login not founded";
                        NotifyPropertyChanged("LoginError");
                        break;
                    case -1:
                        LoginError = "Connection error";
                        NotifyPropertyChanged("LoginError");
                        break;
                    default:
                        if (time == 50)
                        {
                            LoginError = "No connection";
                            NotifyPropertyChanged("LoginError");
                        }
                        break;
                }
            });
            CheckConnectionTask.Start();
        }
        #endregion

        #region Commands
        public RelayCommand<Window> ConfirmButton { get; set; }

        public Command Registrate_LoginButton { get; set; }

        public Command ReestablishPassword { get; set; }


        private RelayCommand<Window> CloseWindowCommand { get; set; }
        public RelayCommand<Window> CloseWindow
        {
            get
            {
                if (this.CloseWindowCommand == null)
                {
                    CloseWindowCommand = new RelayCommand<Window>(OnCloseWindow);
                }
                return CloseWindowCommand;
            }
        }

        //For password binding

        private PasswordBox pb;
        public ICommand PasswordChangedCommand
        {
            get
            {
                return new RelayCommand<object>(ExecChangePassword);
            }
        }

        private void ExecChangePassword(object obj)
        {
            if (pb == null)
            {
                pb = (PasswordBox)obj;
            }
            Password = ((PasswordBox)obj).Password;
        }

        #endregion


        #region ImplementationCommand
        private void OnCloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        public void OnLogin(Window loginWindow)
        {
            if (LoginWindow == null)          //initialization login window
                LoginWindow = loginWindow;
        }

        public void OnRegistrate(Window loginWindow)
        {
            if (LoginWindow == null)          //initialization login window
                LoginWindow = loginWindow;
        }

        public void OnReestablish(Window loginWindow)
        {
            if (LoginWindow == null)          //initialization login window
                LoginWindow = loginWindow;
        }

        public void ChangeOnRegistration()
        {
            PageMode = Mode.Registrate;
        }

        public void ChangeOnLogIn()
        {
            PageMode = Mode.LogIn;
        }

        public void ChangeOnReestablishPassword()
        {
            PageMode = Mode.ReestablishPassword;
        }




        //public void OnLogIn(Window loginWindow)
        //{
        //    if (LoginWindow == null)          //initialization login window
        //        LoginWindow = loginWindow;

        //    LoginError = PasswordError = "";
        //    User TargetUser = null;

        //    if (_login != "" && _login != null)
        //    {
        //        if (Helper.users.Exists(x => x.Login == _login))    //check on exist
        //        {
        //            TargetUser = Helper.users.Find(x => x.Login == _login);          //set user
        //        }
        //        else
        //        {
        //            LoginError = "Login doesn't exist";
        //        }
        //    }
        //    else
        //    {
        //        LoginError = "Login is empty";
        //    }
        //    if (_password != "" && _password != null)
        //    {
        //        if (TargetUser != null)
        //        {
        //            if (TargetUser.Password == PasswordCoder.PasswordCoder.GetHash(Password))             //check user password with input password
        //            {
        //                if (MainWindow == null)
        //                    MainWindow = new MainWindow();

        //                MainViewModel.GetContext.CurrentUser = TargetUser;

        //                MainViewModel.GetContext._pages[0] = new HomePage();
        //                MainViewModel.GetContext._pages[1] = new TargetsPage();
        //                MainViewModel.GetContext._pages[2] = new CategoriesPage();
        //                MainViewModel.GetContext._pages[3] = new IncomesPage();
        //                MainViewModel.GetContext._pages[4] = new TransactionsPage();
        //                MainViewModel.GetContext._pages[5] = new StatisticPage();
        //                MainViewModel.GetContext._pages[6] = new SettingsPage();
        //                MainViewModel.GetContext._pages[7] = new NotePage();

        //                MainViewModel.GetContext.OnUpdateMainWindow();
        //                if (MainViewModel.mainWindow == null)               //set static properties in main view-model
        //                {
        //                    MainViewModel.loginWindow = LoginWindow;
        //                    MainViewModel.mainWindow = MainWindow;
        //                }
        //                MainViewModel.GetContext.UpdateInfo();

        //                LoginWindow.Hide();
        //                MainWindow.Show();
        //            }
        //            else
        //            {
        //                PasswordError = "Password is wrong";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        PasswordError = "Password is empty";
        //    }
        //    NotifyPropertyChanged("LoginError");
        //    NotifyPropertyChanged("PasswordError");
        //}

        //public void OnRegistrate()
        //{
        //    LoginError = PasswordError = "";
        //    User TargetUser = null;
        //    if (_login != "" && _login != null)
        //    {
        //        if (!Helper.users.Exists(x => x.Login == _login))       //check on existing
        //        {
        //            TargetUser = new User(_login, _password, 0, DateTime.Now);              //create new user object
        //        }
        //        else
        //        {
        //            LoginError = "Login exists";
        //        }
        //    }
        //    else
        //    {
        //        LoginError = "Login is empty";
        //    }
        //    if (_password != "" && _password != null)
        //    {
        //        if (TargetUser != null)
        //        {
        //            if (Password.Length >= 6)                   // validate password 
        //            {
        //                TargetUser.Password = PasswordCoder.PasswordCoder.GetHash(TargetUser.Password);
        //                new Task(() =>
        //                {
        //                    unitOfWork.Users.Create(TargetUser);                          //push to database
        //                    unitOfWork.Save();
        //                    Helper.GetUsers();
        //                }).Start();

        //                MessageBox.Show($"{_login} is registered");
        //            }
        //            else
        //            {
        //                PasswordError = "Password is too short";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        PasswordError = "Password is empty";
        //    }
        //    NotifyPropertyChanged("LoginError");
        //    NotifyPropertyChanged("PasswordError");
        //}
        #endregion
    }
}

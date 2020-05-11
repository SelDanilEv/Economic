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

namespace Economic_v2.ViewModels
{
    class LoginViewModel : ViewModelBase
    {
        private UnitOfWork unitOfWork;   // pattern for work with database

        public static Window MainWindow;   //reference on windows for working
        public static Window LoginWindow;

        public LoginViewModel()
        {
            unitOfWork = UnitOfWorkSingleton.GetUnitOfWork;
            Helper.GetUsers();
        }

        #region View   
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
                LoginError = "";
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
                PasswordError = "";
            }
        }
        public string LoginError { get; set; }
        public string PasswordError { get; set; }
        #endregion

        #region Commands
        public RelayCommand<Window> LogInCommand { get; private set; }
        public RelayCommand<Window> LogIn
        {
            get
            {
                if (this.LogInCommand == null)
                {
                    LogInCommand = new RelayCommand<Window>(OnLogIn);
                }
                return LogInCommand;
            }
        }

        private Command RegistrateCommand;
        public Command Registrate
        {
            get
            {
                if (this.RegistrateCommand == null)
                {
                    RegistrateCommand = new Command(OnRegistrate);
                }
                return RegistrateCommand;
            }
        }

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

        private void OnCloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        //For password binding
        public ICommand PasswordChangedCommand
        {
            get
            {
                return new RelayCommand<object>(ExecChangePassword);
            }
        }

        private void ExecChangePassword(object obj)
        {
            Password = ((System.Windows.Controls.PasswordBox)obj).Password;
        }

        public void OnLogIn(Window loginWindow)
        {
            if (LoginWindow == null)          //initialization login window
                LoginWindow = loginWindow;

            LoginError = PasswordError = "";
            User TargetUser = null;

            if (_login != "" && _login != null)
            {
                if (Helper.users.Exists(x => x.Login == _login))    //check on exist
                {
                    TargetUser = Helper.users.Find(x => x.Login == _login);          //set user
                }
                else
                {
                    LoginError = "Login doesn't exist";
                }
            }
            else
            {
                LoginError = "Login is empty";
            }
            if (_password != "" && _password != null)
            {
                if (TargetUser != null)
                {
                    if (TargetUser.Password == PasswordEncoder.GetHash(Password))             //check user password with input password
                    {
                        MainViewModel.CurrentUser = TargetUser;

                        if (MainWindow == null)
                            MainWindow = new MainWindow();
                        if (MainViewModel.mainWindow == null)               //set static properties in main view-model
                        {
                            MainViewModel.loginWindow = LoginWindow;
                            MainViewModel.mainWindow = MainWindow;
                        }

                        LoginWindow.Hide();
                        MainWindow.Show();
                    }
                    else
                    {
                        PasswordError = "Password is wrong";
                    }
                }
            }
            else
            {
                PasswordError = "Password is empty";
            }
            NotifyPropertyChanged("LoginError");
            NotifyPropertyChanged("PasswordError");
        }

        public void OnRegistrate()
        {
            LoginError = PasswordError = "";
            User TargetUser = null;
            if (_login != "" && _login != null)
            {
                if (!Helper.users.Exists(x => x.Login == _login))       //check on existing
                {
                    TargetUser = new User(_login, _password, 0, DateTime.Now);              //create new user object
                }
                else
                {
                    LoginError = "Login exists";
                }
            }
            else
            {
                LoginError = "Login is empty";
            }
            if (_password != "" && _password != null)
            {
                if (TargetUser != null)
                {
                    if (Password.Length >= 6)                   // validate password 
                    {
                        TargetUser.Password = PasswordEncoder.GetHash(TargetUser.Password);
                        new Task(()=> 
                        {
                            unitOfWork.Users.Create(TargetUser);                          //push to database
                            unitOfWork.Save();
                            Helper.GetUsers();
                        }).Start();

                        MessageBox.Show($"{_login} is registered");
                    }
                    else
                    {
                        PasswordError = "Password is too short";
                    }
                }
            }
            else
            {
                PasswordError = "Password is empty";
            }
            NotifyPropertyChanged("LoginError");
            NotifyPropertyChanged("PasswordError");
        }
        #endregion
    }
}

using Economic_v2.DataBaseLayer;
using Economic_v2.Commands;
using System.Windows.Input;
using Economic_v2.Models;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Economic_v2.Pages;
using Economic_v2.Windows;
using System;
using Economic_v2.Services;

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
        public bool IsPasswordEnable
        {
            get
            {
                if (PageMode == Mode.ReestablishPassword)
                    return false;
                return true;
            }
        }

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
                        HeaderText = "LOG IN";
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
                NotifyPropertyChanged("IsPasswordEnable");
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
                if (validateLogin(value))
                {
                    GetUserByLogin(value);
                }
                NotifyPropertyChanged("LoginError");
                _login = value;
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
            new Task(() =>
            {
                _login = _password = "";

                NotifyPropertyChanged("Login");
                NotifyPropertyChanged("Password");
                NotifyPropertyChanged("PasswordError");
                NotifyPropertyChanged("LoginError");
            }).Start();

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                if (pb != null)
                    pb.Password = "";
            });
            PasswordError = LoginError = null;
        }
        #endregion

        #region Validation
        private bool validateLogin(string login)
        {
            if (login == "")
            {
                LoginError = "Login is empty";
                return false;
            }

            if (login.Length < 2)
            {
                LoginError = "Login too short";
                return false;
            }
            if (login[0] == ' ')
            {
                LoginError = "Login mustn't starts with space";
                return false;
            }

            LoginError = null;
            return true;
        }

        private bool validateEmail()
        {
            if (TargetUser != null)
            {
                if (TargetUser.Email != null && TargetUser.Email != "")
                {
                    LoginError = null;
                    return true;
                }
            }

            LoginError = "Not email binding";
            return false;
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

        private bool checkPasswords(string password)
        {
            if (TargetUser != null)
                if (PasswordCoder.PasswordCoder.GetHash(password) == TargetUser.Password)
                    return true;
            return false;
        }
        #endregion

        #region Get Check User region
        private User TargetUser = null;
        private void GetUserByLogin(string login)
        {
            (new Task(() =>
            {
                if (CheckConnectionTask.Status == TaskStatus.Running)
                {
                    time = 999;
                    Thread.Sleep(50);
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
                TurnOnLoadAnimation();

                while (time < 300 && TargetUser == null)
                {
                    Thread.Sleep(50);
                    time++;
                }

                TurnOffLoadAnimation();

                switch (PageMode)
                {
                    case Mode.LogIn:
                    case Mode.ReestablishPassword:
                        if (TargetUser == null)
                        {
                            LoginError = "Connection error";
                            NotifyPropertyChanged("LoginError");
                        }
                        else
                        {
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
                                    if (time == 300)
                                    {
                                        LoginError = "No connection";
                                        NotifyPropertyChanged("LoginError");
                                    }
                                    break;
                            }
                        }
                        break;
                    case Mode.Registrate:
                        if (TargetUser == null)
                        {
                            LoginError = "Connection error";
                            NotifyPropertyChanged("LoginError");
                        }
                        else
                        {
                            switch (TargetUser.Id)
                            {
                                case -1:
                                    LoginError = "Connection error";
                                    NotifyPropertyChanged("LoginError");
                                    break;
                                default:
                                    if (time == 300)
                                    {
                                        LoginError = "No connection";
                                        NotifyPropertyChanged("LoginError");
                                    }
                                    if (TargetUser.Id > 0)
                                    {
                                        LoginError = "Login already exist";
                                        NotifyPropertyChanged("LoginError");
                                    }
                                    break;
                            }
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

        #region only chec connect
        public void CheckConnectionOnly()
        {
            new Task(() =>
            {
                User tmp = UnitOfWorkSingleton.GetUnitOfWork.Users.GetPure("");
                while (time < 200 && tmp == null)
                {
                    Thread.Sleep(50);
                    time++;
                }

                if (tmp == null)
                {
                    LoginError = "Connection error (1)";
                    NotifyPropertyChanged("LoginError");
                }
                else
                {
                    switch (tmp.Id)
                    {
                        case -1:
                            LoginError = "Connection error (2)";
                            NotifyPropertyChanged("LoginError");
                            break;
                        default:
                            if (time == 200)
                            {
                                LoginError = "No connection";
                                NotifyPropertyChanged("LoginError");
                            }
                            break;
                    }
                }
            }).Start();
        }
        #endregion

        #region Animation
        private void TurnOnLoadAnimation()
        {
            new Task(() =>
            {
                LoadVis = "Visible";
                NotifyPropertyChanged("LoadVis");
            }).Start();
        }

        private void TurnOffLoadAnimation()
        {
            new Task(() =>
            {
                LoadVis = "Hidden";
                NotifyPropertyChanged("LoadVis");
            }).Start();
        }
        #endregion

        #region ImplementationCommand
        private void OnCloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
            if (MainWindow != null)
            {
                MainWindow.Close();
            }
        }

        #region Log In
        private bool isTry;
        public void OnLogin(Window loginWindow)
        {
            if (LoginWindow == null)          //initialization login window
                LoginWindow = loginWindow;
            TurnOnLoadAnimation();
            if (!isTry)
            {
                new Task(() =>
                {
                    if (TargetUser != null && TargetUser.Id > 0 && validateLogin(_login) && validatePassword())
                    {
                        DoCheckValidateAccount();
                    }
                    else
                    {
                        if (validatePassword())
                        {
                            new Task(() =>
                            {
                                isTry = true;
                                bool OK = false;
                                for (int i = 0; i < 30; i++)
                                {
                                    if (TargetUser != null && TargetUser.Id > 0 && validateLogin(_login) && validatePassword())
                                    {
                                        OK = true;
                                        DoCheckValidateAccount();
                                        isTry = false;
                                        break;
                                    }
                                    Thread.Sleep(100);
                                }
                                isTry = false;
                                if (!OK)
                                {
                                    LoginError = "Please try again";
                                    NotifyPropertyChanged("LoginError");
                                }
                            }).Start();
                        }
                        else
                        {
                            TurnOffLoadAnimation();
                        }
                    }
                }).Start();
            }
        }
        public void DoCheckValidateAccount()
        {
            if (checkPasswords(_password))
            {
                HeaderText = "Welcome";
                NotifyPropertyChanged("HeaderText");
                HelpMethodWait();
            }
            else
            {
                PasswordError = "Wrong password";
                NotifyPropertyChanged("PasswordError");
            }
        }
        public void HelpMethodWait()
        {
            new Task(() =>
            {
                Thread.Sleep(1100);
                OpenAccount();
                isTry = false;
            }).Start();
        }
        public void OpenAccount()
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                if (MainWindow == null)
                    MainWindow = new MainWindow();

                if (MainViewModel.mainWindow == null)               //set static properties in main view-model
                {
                    MainViewModel.loginWindow = LoginWindow;
                    MainViewModel.mainWindow = MainWindow;
                }

                MainViewModel.GetContext.CurrentUser =
                    UnitOfWorkSingleton.GetUnitOfWork.Users.Get(TargetUser.Login);

                MainViewModel.GetContext._pages[0] = new HomePage();
                MainViewModel.GetContext._pages[1] = new TargetsPage();
                MainViewModel.GetContext._pages[2] = new CategoriesPage();
                MainViewModel.GetContext._pages[3] = new IncomesPage();
                MainViewModel.GetContext._pages[4] = new TransactionsPage();
                MainViewModel.GetContext._pages[5] = new StatisticPage();
                MainViewModel.GetContext._pages[6] = new SettingsPage();
                MainViewModel.GetContext._pages[7] = new NotePage();

                MainViewModel.GetContext.UpdateInfo();
                MainViewModel.GetContext.SelectedPageIndex = 0;
                MainViewModel.GetContext.OnUpdateMainWindow();

                ChangeOnLogIn();

                LoginWindow.Hide();
                MainWindow.Show();
            });
            TurnOffLoadAnimation();
        }
        #endregion


        public void OnRegistrate(Window loginWindow)
        {
            new Task(() =>
            {
                if (TargetUser != null && TargetUser.Id == 0 && validateLogin(_login) && validatePassword())
                {
                    CreateNewUser();
                }
            }).Start();
        }

        private void CreateNewUser()
        {
            UnitOfWorkSingleton.GetUnitOfWork.Users.Create(new User
                (_login, PasswordCoder.PasswordCoder.GetHash(_password), 0, DateTime.Now));
            UnitOfWorkSingleton.GetUnitOfWork.Save();
            HeaderText = "Hi " + _login;
            NotifyPropertyChanged("HeaderText");
            Thread.Sleep(2000);
            new Task(() =>
            {
                ChangeOnLogIn();
            }).Start();
        }


        public void OnReestablish(Window loginWindow)
        {
            new Task(() =>
            {
                if (TargetUser != null && TargetUser.Id > 0 && validateLogin(_login)&&validateEmail())
                {
                    SendEmail();
                }
                else
                {
                    NotifyPropertyChanged("LoginError");
                }
            }).Start();
        }

        private void SendEmail()
        {
            string genRandPassword = RandomGenerator.GetRandomString(6);
            TargetUser.Password = PasswordCoder.PasswordCoder.GetHash(genRandPassword);
            MailsService.SendEmailAsync(TargetUser.Email, "Reestablish Password","EconoMic",
                "Hi user. Your new password for "+ TargetUser.Login+" is : "+ genRandPassword+"      \n"+
                "Thanks for using my app!\n\t\t(SD)");
            HeaderText = "Check email";
            NotifyPropertyChanged("HeaderText");
            UnitOfWorkSingleton.GetUnitOfWork.Users.Update(TargetUser);
            UnitOfWorkSingleton.GetUnitOfWork.Save();
            Thread.Sleep(2000);
            new Task(() =>
            {
                ChangeOnLogIn();
            }).Start();
        }


        public void ChangeOnRegistration()
        {
            PageMode = Mode.Registrate;
            ClearFields();
        }

        public void ChangeOnLogIn()
        {
            PageMode = Mode.LogIn;
            ClearFields();
        }

        public void ChangeOnReestablishPassword()
        {
            PageMode = Mode.ReestablishPassword;
            ClearFields();
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

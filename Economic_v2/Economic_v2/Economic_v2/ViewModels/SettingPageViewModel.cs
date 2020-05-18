using Economic_v2.DataBaseLayer;
using Economic_v2.Commands;
using System.Windows.Input;
using System.Collections.Generic;
using Economic_v2.Models;
using System.Windows;
using System;
using System.Threading;
using Economic_v2.Windows;
using Economic_v2.PasswordCoder;
using System.Threading.Tasks;

namespace Economic_v2.ViewModels
{
    public class SettingPageViewModel : ViewModelBase
    {
        private static object _context;

        public SettingPageViewModel()
        {
            _context = this;
        }

        public static SettingPageViewModel GetContext
        {
            get => (SettingPageViewModel)_context;
        }

        #region Setup
        public void Setup()
        {
            (new Task(() =>
            {
                OldPasswordError = NewPasswordError = ConfirmNewPasswordError =
                   NewTotalMoneyError = MailError = null;

                NotifyPropertyChanged("ConfirmNewPasswordError");
                NotifyPropertyChanged("NewPasswordError");
                NotifyPropertyChanged("OldPasswordError");
                NotifyPropertyChanged("NewTotalMoneyError");
                NotifyPropertyChanged("MailError");

                NotifyPropertyChanged("OldTotalMoney");
                NotifyPropertyChanged("OldMail");
            })).Start();
        }
        #endregion

        #region View
        private string _oldPassword;
        public string OldPasswordError { get; set; }
        public string OldPassword
        {
            get
            {
                return _oldPassword;
            }
            set
            {
                _oldPassword = value;
                validateOldPassword();
                NotifyPropertyChanged("OldPasswordError");
            }
        }

        private string _newPassword;
        public string NewPasswordError { get; set; }
        public string NewPassword
        {
            get
            {
                return _newPassword;
            }
            set
            {
                _newPassword = value;
                validateNewPassword();
                validateConfirmNewPassword();
                NotifyPropertyChanged("ConfirmNewPasswordError");
                NotifyPropertyChanged("NewPasswordError");
            }
        }

        private string _confirmNewPassword;
        public string ConfirmNewPasswordError { get; set; }
        public string ConfirmNewPassword
        {
            get
            {
                return _confirmNewPassword;
            }
            set
            {
                _confirmNewPassword = value;
                validateConfirmNewPassword();
                NotifyPropertyChanged("ConfirmNewPasswordError");
            }
        }

        public string OldTotalMoney
        {
            get
            {
                if (MainViewModel.GetContext.CurrentUser != null)
                    return MainViewModel.GetContext.CurrentUser.Total_money.ToString();
                return "total money";
            }
        }

        private double? _newTotalMoney = null;
        private string _newTotalMoneystr;
        public string NewTotalMoney
        {
            get
            {
                return _newTotalMoney.ToString();
            }
            set
            {
                try
                {
                    _newTotalMoneystr = value;
                    validateNewTotalMoney();
                }
                finally
                {
                    NotifyPropertyChanged("NewTotalMoneyError");
                }
            }
        }
        public string NewTotalMoneyError { get; set; }

        public string OldMail
        {
            get
            {
                if (MainViewModel.GetContext.CurrentUser != null)
                    if (MainViewModel.GetContext.CurrentUser.Email != null &&
                        MainViewModel.GetContext.CurrentUser.Email != "")
                        return MainViewModel.GetContext.CurrentUser.Email;
                return "Email";
            }
        }
        public string Mail { get; set; }
        public string MailError { get; set; }
        #endregion

        #region Validation
        private bool validateOldPassword()
        {
            if (!PasswordCoder.PasswordCoder.GetHash(_oldPassword).Equals(MainViewModel.GetContext.CurrentUser.Password))
            {
                OldPasswordError = "Wrong password";
                return false;
            }

            OldPasswordError = null;
            return true;
        }

        private bool validateNewPassword()
        {
            if (_newPassword.Length < 6)
            {
                NewPasswordError = "Password too short";
                return false;
            }

            if (_newPassword[0] == ' ')
            {
                NewPasswordError = "Password can't starts with space";
                return false;
            }

            NewPasswordError = null;
            return true;
        }

        private bool validateConfirmNewPassword()
        {
            if (!_newPassword.Equals(_confirmNewPassword))
            {
                ConfirmNewPasswordError = "Passwords aren't equals";
                return false;
            }

            ConfirmNewPasswordError = null;
            return true;
        }

        private bool validateNewTotalMoney()
        {
            try
            {
                _newTotalMoney = double.Parse(_newTotalMoneystr);
            }
            catch
            {
                NewTotalMoneyError = "Value must be number";
                return false;
            }

            if (_newTotalMoneystr != "")
            {
                string[] temp = _newTotalMoney.ToString().Split('.', ',');
                if (temp.Length > 1)
                {
                    if (temp[1].Length > 2)
                    {
                        NewTotalMoneyError = "Money format have two decimal places";
                        return false;
                    }
                }
            }

            if (_newTotalMoney < 0)
            {
                NewTotalMoneyError = "Total money must be positive";
                return false;
            }

            NewTotalMoneyError = null;
            return true;
        }

        private bool validateMail()
        {
            if (!Mail.Contains("@"))
            {
                MailError = "Invalid mail";
                return false;
            }

            MailError = null;
            return true;
        }
        #endregion

        #region Commands
        private Command ChangeTotalMoneyCommand;
        public Command ChangeTotalMoney
        {
            get
            {
                if (ChangeTotalMoneyCommand == null)
                {
                    ChangeTotalMoneyCommand = new Command(OnChangeTotalMoney);
                }
                return ChangeTotalMoneyCommand;
            }
        }

        private Command ChangePasswordCommand;
        public Command ChangePassword
        {
            get
            {
                if (ChangePasswordCommand == null)
                {
                    ChangePasswordCommand = new Command(OnChangePassword);
                }
                return ChangePasswordCommand;
            }
        }

        private Command ConfirmMailCommand;
        public Command ConfirmMail
        {
            get
            {
                if (ConfirmMailCommand == null)
                {
                    ConfirmMailCommand = new Command(OnConfirmMail);
                }
                return ConfirmMailCommand;
            }
        }

        private Command ChangeAccountCommand;
        public Command ChangeAccount
        {
            get
            {
                if (ChangeAccountCommand == null)
                {
                    ChangeAccountCommand = new Command(OnChangeAccount);
                }
                return ChangeAccountCommand;
            }
        }
        #endregion

        #region ImplementationCommand
        private void OnChangePassword()
        {
            if (validateOldPassword() && validateNewPassword() && validateConfirmNewPassword())
            {
                (new Task(() =>
                {
                    OldPassword = NewPassword = ConfirmNewPassword = "";
                    NotifyPropertyChanged("ConfirmNewPassword");
                    NotifyPropertyChanged("NewPassword");
                    NotifyPropertyChanged("OldPassword");
                    MainViewModel.GetContext.CurrentUser.Password = PasswordCoder.PasswordCoder.GetHash(_newPassword);
                    UnitOfWorkSingleton.GetUnitOfWork.Users.Update(MainViewModel.GetContext.CurrentUser);
                    UnitOfWorkSingleton.GetUnitOfWork.Save();
                })).Start();
            }
            NotifyPropertyChanged("ConfirmNewPasswordError");
            NotifyPropertyChanged("NewPasswordError");
            NotifyPropertyChanged("OldPasswordError");
        }

        private void OnChangeTotalMoney()
        {
            if (validateNewTotalMoney())
            {
                (new Task(() =>
                {
                    MainViewModel.GetContext.CurrentUser.Total_money =(double)_newTotalMoney;
                    MainViewModel.GetContext.UpdateInfo();
                    UnitOfWorkSingleton.GetUnitOfWork.Users.Update(MainViewModel.GetContext.CurrentUser);
                    UnitOfWorkSingleton.GetUnitOfWork.Save();
                })).Start();
            }
            NotifyPropertyChanged("NewTotalMoneyError");
        }

        private void OnConfirmMail()
        {
            if (validateMail())
            {
                (new Task(() =>
                {
                    MainViewModel.GetContext.CurrentUser.Email = Mail;
                    UnitOfWorkSingleton.GetUnitOfWork.Users.Update(MainViewModel.GetContext.CurrentUser);
                    UnitOfWorkSingleton.GetUnitOfWork.Save();
                })).Start();
            }
            NotifyPropertyChanged("MailError");
        }

        private void OnChangeAccount()
        {
            MainViewModel.mainWindow.Hide();
            MainViewModel.loginWindow.Show();
        }
        #endregion
    }
}

using EconoMiC.Data;
using EconoMiC.Logic;
using System;
using System.Windows;

namespace EconoMiC
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MoneyController moneyController = new MoneyController();
            moneyController.AddMoney(TempData.GetUser("Danil"),13.56);
            UserAccount userAccount = new UserAccount(TempData.GetUser("Danil"));
            this.Content = userAccount;
        }

        private void ButSignIn_Click(object sender, RoutedEventArgs e)
        {
            string Login = TextBoxLogin.Text;
            string Password = TextBoxPassword.Password;

            if (TempData.validate(Login,Password)) {
                UserAccount userAccount = new UserAccount(TempData.GetUser(Login));
                this.Content = userAccount;
            }
        }

        private void ButRegistrate_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
        }
        
    }
}

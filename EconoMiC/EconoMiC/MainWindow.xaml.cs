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
        }

        private void ButSignIn_Click(object sender, RoutedEventArgs e)
        {
            string Login = TextBoxLogin.Text;
            string Password = TextBoxPassword.Password;

            if (Data.Data.validate(Login,Password))
            {
                UserAccount userAccount = new UserAccount(Data.Data.GetUser(Login, Password));
                this.Content = userAccount;
            }
        }

        private void ButRegistrate_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
        }
    }
}

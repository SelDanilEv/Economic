using EconoMiC.Data;
using EconoMiC.Logic;
using System;
using System.Windows;

namespace EconoMiC
{
    public partial class MainWindow : Window
    {
        Views.ViewModelMain ViewModelMain;

        public MainWindow()
        {
            InitializeComponent();
            ViewModelMain = new Views.ViewModelMain(this);
        }

        private void ButSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (Data.Data.validate(ViewModelMain))
            {
                UserAccount userAccount = new UserAccount(Data.Data.GetUser(ViewModelMain.GetLogin,ViewModelMain.GetPassword));
                this.Content = userAccount;
            }
        }

        private void ButRegistrate_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
        }
    }
}

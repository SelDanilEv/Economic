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
        }

        private void ButRegistrate_Click(object sender, RoutedEventArgs e)
        {
            TextBoxLogin.AppendText(" F ");
        }
    }
}

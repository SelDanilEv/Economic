using EconoMiC.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EconoMiC
{
    /// <summary>
    /// Логика взаимодействия для UserAccount.xaml
    /// </summary>
    public partial class UserAccount : Page
    {
        public User user;

        public UserAccount(User user)
        {
            InitializeComponent();
            UpdateUser(user);
        }

        public UserAccount()
        {
            InitializeComponent();
        }

        public void UpdateUser(User user)
        {
            this.user = user;
            LabelUserName.Content = user.Login;
            LabelFreeMoney.Content = user.Money;
        }
        public void UpdateUser()
        {
            LabelUserName.Content = user.Login;
            LabelFreeMoney.Content = user.Money;
        }
    }
}

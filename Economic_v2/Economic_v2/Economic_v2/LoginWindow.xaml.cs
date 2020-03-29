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
using System.Windows.Shapes;

namespace Economic_v2
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MyDataBaseContext myDataBaseContext = new MyDataBaseContext();

            //User user = new User(1, "Danil", "Danil", 10.10, DateTime.Now);
            //user.Categories = new List<Category>
            //{
            //    new Category("Eat",5.5)
            //};
            //user.Incomes = new List<Income>
            //{
            //    new Income("Stipuha",150,DateTime.Parse("10/10/2000"))
            //};
            //user.Targets = new List<Target>
            //{
            //    new Target("Car",5000,10,300)
            //};
            //myDataBaseContext.Users.Add(user);
            //myDataBaseContext.SaveChanges();

            TempData tempData = new TempData(this);
            TempData.GetWindow().Hide();
            new MainWindow().ShowDialog();
        }
    }
}

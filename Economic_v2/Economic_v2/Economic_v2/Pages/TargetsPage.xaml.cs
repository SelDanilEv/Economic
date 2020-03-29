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

namespace Economic_v2
{
    /// <summary>
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class TargetsPage : UserControl
    {
        public TargetsPage()
        {
            InitializeComponent();
            List<Target> items = new List<Target>();

            items.Add(new Target("Car", 100, 10.10, 10));
            items.Add(new Target("Laptop", 10000, 100.10, 100));

            Targets.ItemsSource = items;
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            var item = Targets.SelectedItem;
            if (item != null)
            {
                MessageBox.Show(((Target)item).TargetName);
            }
        }
    }
}

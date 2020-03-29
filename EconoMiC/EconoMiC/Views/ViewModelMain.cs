using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconoMiC.Views
{
    class ViewModelMain
    {
        MainWindow MainWindow;

        public ViewModelMain(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }

        public string GetLogin { get { return MainWindow.TextBoxLogin.Text; } set { } }
        public string GetPassword { get { return MainWindow.TextBoxPassword.Password; } set { } }
    }
}

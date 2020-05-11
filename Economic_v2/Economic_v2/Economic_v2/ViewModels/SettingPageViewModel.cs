using Economic_v2.DataBaseLayer;
using Economic_v2.Commands;
using System.Windows.Input;
using System.Collections.Generic;
using Economic_v2.Models;
using System.Windows;
using System;
using System.Threading;
using Economic_v2.Windows;

namespace Economic_v2.ViewModels
{
    public class SettingPageViewModel : ViewModelBase
    {
        public SettingPageViewModel()
        {

        }

        private string _testString = "test";
        public string TestString { get => _testString; set {_testString = value; NotifyPropertyChanged(); } }
    }
}

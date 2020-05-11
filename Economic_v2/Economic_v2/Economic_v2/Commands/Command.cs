using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Economic_v2.Commands
{
    public class RelayCommand<T> : ICommand        //command with parameter
    {
        private Action<T> _execute;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke((T)parameter);
        }

        public RelayCommand(Action<T> execute)
        {
            _execute = execute;
        }
    }

    public class Command : ICommand              // command without parameter
    {
        public Command() { }

        private Action _execute;

        public event EventHandler CanExecuteChanged;

        public Command(Action execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke();
        }
    }
}

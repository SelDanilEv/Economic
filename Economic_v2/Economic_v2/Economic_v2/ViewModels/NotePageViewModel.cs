using Economic_v2.DataBaseLayer;
using Economic_v2.Commands;
using System.Windows.Input;
using System.Collections.Generic;
using Economic_v2.Models;
using System.Windows;
using System;
using System.Threading;
using Economic_v2.Windows;
using System.Threading.Tasks;

namespace Economic_v2.ViewModels
{
    public class NotePageViewModel : ViewModelBase
    {
        private static object _context;

        public NotePageViewModel()
        {
            _context = this;
        }

        public static NotePageViewModel GetContext
        {
            get => (NotePageViewModel)_context;
        }

        private bool _isReadOnly = true;
        public bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                _isReadOnly = value;
                NotifyPropertyChanged("IsReadOnly");
            }
        }
        public string UserNote
        {
            get
            {
                return MainViewModel.GetContext.CurrentUser.Note.text;
            }
            set
            {
                MainViewModel.GetContext.CurrentUser.Note.text = value;
                (new Task(() =>
                {
                    UnitOfWorkSingleton.GetUnitOfWork.Notes.Update(MainViewModel.GetContext.CurrentUser.Note);
                    UnitOfWorkSingleton.GetUnitOfWork.Save();
                })).Start();
            }
        }
    }
}

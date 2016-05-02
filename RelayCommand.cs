using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DLeh.Util.MVVM
{
    public class RelayCommand : ICommand
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        //private Action<object> action;


        /// <summary>
        /// Create a RelayCommand that doesn't expect a parameter
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action execute)
            : this(new Action<object>(x => execute()), null) { }

        /// <summary>
        /// Create a RelayCommand that doesn't expect a parameter
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action execute, Predicate<object> canExecute)
            : this(new Action<object>(x => execute()), canExecute) { }

        public RelayCommand(Action<object> execute)
            : this(execute, null) { }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            //this should never happen, because there's no constructors that allow it.
            //besides, it'll throw a null error anyway
            //if (execute == null) 
            //    throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

    }
}

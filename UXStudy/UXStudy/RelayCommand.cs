using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UXStudy
{
    //this is another class I port to nearly every project. This allows the binding of any action to the Command property
    //of a Button in WPF.
    public class RelayCommand : ICommand
    {
        private Predicate<object> canExecute;
        private Action execute;

        public RelayCommand(Action ex, Predicate<object> canEx = null)
        {
            execute = ex;
            canExecute = canEx;
        }

        //checks whether an action can be executed
        public bool CanExecute(object parameter)
        {
            return (canExecute == null) ? true : canExecute(parameter);
        }

        //executes the action
        public void Execute(object parameter)
        {
            execute();
        }

        //im not sure what exactly this does, but it is needed to work
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}

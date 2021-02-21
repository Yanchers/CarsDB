using System;
using System.Windows.Input;

namespace CourseProjectDataBaseCars
{
    public class RelayCommand : ICommand
    {
        private Action<object> mAction;
        private Func<object, bool> mCanExecute;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public RelayCommand(Action<object> action, Func<object, bool> canExecute = null)
        {
            mAction = action;
            mCanExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object param) => mAction(param);
    }
}

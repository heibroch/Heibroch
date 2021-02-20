using System;
using System.Windows.Input;

namespace Heibroch.Common.Wpf
{
    public class ActionCommand : ICommand
    {
        private readonly Action<object> action;

        public ActionCommand(Action<object> action) => this.action = action;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => action(parameter);

        public event EventHandler CanExecuteChanged;
    }
}

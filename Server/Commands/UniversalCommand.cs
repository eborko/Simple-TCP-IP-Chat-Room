using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Server.Commands
{
    public class UniversalCommand : ICommand
    {
        private readonly Action _TargetExecuteMethod;
        private readonly Func<bool> _TargetCanExecuteMethod;

        public UniversalCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            _TargetCanExecuteMethod = canExecuteMethod;
            _TargetExecuteMethod = executeMethod;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            if (_TargetCanExecuteMethod != null)
                return _TargetCanExecuteMethod();
            
            return false;
        }

        public void Execute(object? parameter)
        {
            if (_TargetExecuteMethod != null)
                _TargetExecuteMethod();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContratosMVVM.Generics
{
    public abstract class AsyncCommandBase : ICommand
    {
        private bool _isExecuting;
        private readonly Action<Exception> _onException;

        public bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                _isExecuting = value;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }

        public AsyncCommandBase(Action<Exception> onException)
        {
            _onException = onException;
        }
        public virtual event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter)
        {
            return !IsExecuting;
        }

        public async void Execute(object parameter)
        {
            IsExecuting = true;
            try
            {
                await ExecuteAsync(parameter);
            }
            catch (Exception ex)
            {
                _onException(ex);
            }
            IsExecuting = false;
        }

        protected abstract Task ExecuteAsync(object parameter);
    }
}

using System;
using System.Windows.Input;

namespace Note.InkCanvasEx.Commons
{
    public class RelayCommand<T> : ICommand
	{
		private readonly Action<T> _execute;
		private readonly Func<T, bool> _canExecute;
		public RelayCommand(Action<T> execute) : this(execute, null)
		{
		}
		public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
		{
			if (execute == null)
			{
				throw new ArgumentNullException("execute");
			}
			this._execute = execute;
			this._canExecute = canExecute;
		}
		public bool CanExecute(object parameter)
		{
			return (this._canExecute == null ? true : this._canExecute((T)parameter));
		}
		public void Execute(object parameter)
		{
			this._execute((T)parameter);
		}
		public void RaiseCanExecuteChanged()
		{
			EventHandler eventHandler = this.CanExecuteChanged;
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}
		public event EventHandler CanExecuteChanged;
	}
}
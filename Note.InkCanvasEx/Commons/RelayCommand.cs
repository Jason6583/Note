using System;
using System.Windows.Input;

namespace Note.InkCanvasEx.Commons
{
    public class RelayCommand : ICommand
	{
		private readonly Action _execute;
		private readonly Func<bool> _canExecute;
		public RelayCommand(Action execute) : this(execute, null)
		{
		}
		public RelayCommand(Action execute, Func<bool> canExecute)
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
			return (this._canExecute == null ? true : this._canExecute());
		}
		public void Execute(object parameter)
		{
			this._execute();
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
using System;
using System.Windows.Input;

namespace WpfClient.Commands
{
	internal class ButtonCommand : ICommand
	{
		private Action<object> executableAction;
		private Func<object, bool> predicate;

		public ButtonCommand(Action<object> action, Func<object, bool> pred)
		{
			executableAction = action;
			predicate = pred;
		}

		public ButtonCommand(Action<object> action)
		{
			executableAction = action;
			predicate = (x => true); // Default predicate
		}

		public bool CanExecute(object parameter)
		{
			return predicate(parameter);
		}

		public event EventHandler CanExecuteChanged;

		public void Execute(object parameter)
		{
			executableAction(parameter);
		}
	}
}

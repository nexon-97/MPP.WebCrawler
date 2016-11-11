using System;
using System.Threading.Tasks;

namespace WpfClient.Commands
{
	using AsyncAction = Func<object, Task>;

	internal class ButtonCommandAsync : IAsyncCommand
	{
		private AsyncAction executableAction;
		private Func<object, bool> predicate;

		public ButtonCommandAsync(AsyncAction action, Func<object, bool> pred)
		{
			executableAction = action;
			predicate = pred;
		}

		public ButtonCommandAsync(AsyncAction action)
		{
			executableAction = action;
			predicate = (x => true); // Default predicate
		}

		public bool CanExecute(object parameter)
		{
			return predicate(parameter);
		}

		public event EventHandler CanExecuteChanged;

		public async void Execute(object parameter)
		{
			await ExecuteAsync(parameter);
		}

		public Task ExecuteAsync(object parameter)
		{
			return executableAction(parameter);
		}
	}
}

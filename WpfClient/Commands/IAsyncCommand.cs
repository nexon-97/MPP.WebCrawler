using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfClient.Commands
{
	internal interface IAsyncCommand : ICommand
	{
		Task ExecuteAsync(object parameter);
	}
}

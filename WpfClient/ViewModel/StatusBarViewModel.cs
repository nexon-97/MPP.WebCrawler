using WpfClient.Model;
using System.Windows;

namespace WpfClient.ViewModel
{
	public class StatusBarViewModel : IViewModel
	{
		public StatusBarModel StatusBar { get; set; }

		public StatusBarViewModel()
		{
			StatusBar = new StatusBarModel();
		}

		public void SetStatus(string text, StatusBarModel.MessageType messageType)
		{
			StatusBar.StatusMessageType = messageType;
			StatusBar.StatusText = text;
		}

		public void BindContext(FrameworkElement element)
		{
			element.DataContext = StatusBar;
		}
	}
}
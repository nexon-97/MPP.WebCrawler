using WpfClient.Model;

namespace WpfClient.ViewModel
{
	public class StatusBarViewModel
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
	}
}
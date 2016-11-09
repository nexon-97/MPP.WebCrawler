using System.Windows;

namespace WpfClient.ViewModel
{
	public class LoggerViewModel : IViewModel
	{
		public LoggerModel Logger { get; set; }

		public LoggerViewModel()
		{
			Logger = new LoggerModel();
		}

		public void BindContext(FrameworkElement element)
		{
			element.DataContext = Logger;
		}

		public void AddLogLine(string line)
		{
			Logger.AddLogLine(line);
		}
	}
}

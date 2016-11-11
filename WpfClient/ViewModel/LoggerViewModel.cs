using System.Collections.Generic;

namespace WpfClient.ViewModel
{
	public class LoggerViewModel : BaseViewModel
	{
		#region Fields
		private List<string> logLines;
		#endregion

		#region Properties
		public string LogText
		{
			get
			{
				return string.Join("\n", logLines);
			}
		}
		#endregion

		public LoggerViewModel()
		{
			logLines = new List<string>();
		}

		public void LogMessage(string message)
		{
			logLines.Add(message);

			RaisePropertyChanged("LogText");
		}
	}
}

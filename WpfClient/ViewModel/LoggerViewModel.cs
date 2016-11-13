using System.Collections.Generic;
using WebCrawler;

namespace WpfClient.ViewModel
{
	internal class LoggerViewModel : BaseViewModel, ILogger
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

		#region Singleton pattern
		private static LoggerViewModel instance;
		private static object instanceLock = new object();

		public static LoggerViewModel Instance
		{
			get
			{
				lock (instanceLock)
				{
					return instance;
				}
			}
		}
		#endregion

		public LoggerViewModel()
			: base(ViewModelId.Logger)
		{
			instance = this;

			logLines = new List<string>();
		}

		public void LogMessage(string message)
		{
			logLines.Add(message);

			RaisePropertyChanged(nameof(LogText));
		}
	}
}

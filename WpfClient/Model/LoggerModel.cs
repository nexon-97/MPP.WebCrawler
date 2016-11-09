using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WpfClient
{
	public class LoggerModel : INotifyPropertyChanged
	{
		private ObservableCollection<string> logLines;

		#region Properties
		public ObservableCollection<string> LogLines
		{
			get
			{
				return logLines;
			}
		}
		#endregion

		public LoggerModel()
		{
			logLines = new ObservableCollection<string>();
		}

		#region INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;

		private void RaisePropertyChanged(string property)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}
		#endregion

		public void AddLogLine(string line)
		{
			LogLines.Add(line);
		}
	}
}

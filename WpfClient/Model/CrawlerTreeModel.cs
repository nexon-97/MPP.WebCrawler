using System;
using System.ComponentModel;

namespace WpfClient.Model
{
	public class CrawlerTreeModel : INotifyPropertyChanged
	{
		#region Fields
		private bool startBtnEnabled;
		private bool stopBtnEnabled;
		#endregion

		#region Properties
		public bool StartBtnEnabled
		{
			get
			{
				return startBtnEnabled;
			}
			set
			{
				startBtnEnabled = value;
				RaisePropertyChanged("StartBtnEnabled");
			}
		}

		public bool StopBtnEnabled
		{
			get
			{
				return stopBtnEnabled;
			}
			set
			{
				stopBtnEnabled = value;
				RaisePropertyChanged("StopBtnEnabled");
			}
		}
		#endregion

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
	}
}

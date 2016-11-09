using System.ComponentModel;
using System.Windows.Media;

namespace WpfClient.Model
{
	public class StatusBarModel : INotifyPropertyChanged
	{
		private string statusText;
		private SolidColorBrush backgroundColor;
		private SolidColorBrush textColor;

		#region Properties
		public string StatusText
		{
			get
			{
				return statusText;
			}
			set
			{
				if (statusText != value)
				{
					statusText = value;

					RaisePropertyChanged("StatusText");
				}
			}
		}

		public SolidColorBrush BackgroundColor
		{
			get
			{
				return backgroundColor;
			}
			set
			{
				if (backgroundColor != value)
				{
					backgroundColor = value;

					RaisePropertyChanged("BackgroundColor");
				}
			}
		}

		public SolidColorBrush TextColor
		{
			get
			{
				return textColor;
			}
			set
			{
				if (textColor != value)
				{
					textColor = value;

					RaisePropertyChanged("TextColor");
				}
			}
		}
		#endregion

		public event PropertyChangedEventHandler PropertyChanged;

		private void RaisePropertyChanged(string property)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}
	}
}

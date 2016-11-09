using System.ComponentModel;
using System.Windows.Media;

namespace WpfClient.Model
{
	public class StatusBarModel : INotifyPropertyChanged
	{
		public enum MessageType
		{
			Info,
			Error,
			Warning
		}

		private string statusText;
		private SolidColorBrush backgroundColor;
		private SolidColorBrush textColor;
		private MessageType messageType;

		private Color messageInfoColor = Color.FromRgb(200, 230, 255);
		private Color messageErrorColor = Colors.DarkRed;
		private Color messageWarningColor = Colors.Yellow;

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

		public MessageType StatusMessageType
		{
			get
			{
				return messageType;
			}
			set
			{
				messageType = value;

				switch (messageType)
				{
					case MessageType.Info:
						BackgroundColor = new SolidColorBrush(messageInfoColor);
						TextColor = new SolidColorBrush(Colors.Black);
						break;
					case MessageType.Error:
						BackgroundColor = new SolidColorBrush(messageErrorColor);
						TextColor = new SolidColorBrush(Colors.White);
						break;
					case MessageType.Warning:
						BackgroundColor = new SolidColorBrush(messageWarningColor);
						TextColor = new SolidColorBrush(Colors.Black);
						break;
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

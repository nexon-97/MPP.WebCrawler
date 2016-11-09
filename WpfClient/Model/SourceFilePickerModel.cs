using System.ComponentModel;

namespace WpfClient.Model
{
	public class SourceFilePickerModel : INotifyPropertyChanged
	{
		#region Fields
		private string sourceFilePath;
		#endregion

		#region Properties
		public string SourceFilePath
		{
			get
			{
				return sourceFilePath;
			}
			set
			{
				if (sourceFilePath != value)
				{
					sourceFilePath = value;

					RaisePropertyChanged("SourceFilePath");

					ApplicationContext.Instance.CrawlerTreeVM.ValidateSourcePath(sourceFilePath);
				}
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

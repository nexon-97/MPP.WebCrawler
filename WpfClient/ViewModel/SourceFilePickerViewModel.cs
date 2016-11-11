using System.Windows;

namespace WpfClient.ViewModel
{
	public class SourceFilePickerViewModel : BaseViewModel
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

					//ApplicationContext.Instance.CrawlerTreeVM.ValidateSourcePath(sourceFilePath);
				}
			}
		}
		#endregion

		public void ChooseFileFromDialog()
		{
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

			dlg.DefaultExt = ".xml";
			dlg.Filter = "Xml Files (*.xml)|*.xml";

			if (dlg.ShowDialog() != null)
			{
				SourceFilePath = dlg.FileName;

				//ApplicationContext.Instance.LoggerVM.AddLogLine("Source file specified. Can start crawling.");
			}
		}
	}
}

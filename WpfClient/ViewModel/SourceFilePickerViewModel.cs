using WpfClient.Model;
using System.Windows;

namespace WpfClient.ViewModel
{
	public class SourceFilePickerViewModel : IViewModel
	{
		public SourceFilePickerModel FilePicker { get; set; }

		public string SourceFilePath
		{
			get
			{
				return FilePicker.SourceFilePath;
			}
		}

		public SourceFilePickerViewModel()
		{
			FilePicker = new SourceFilePickerModel();
		}

		public void ChooseFileFromDialog()
		{
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

			dlg.DefaultExt = ".xml";
			dlg.Filter = "Xml Files (*.xml)|*.xml";

			if (dlg.ShowDialog() != null)
			{
				FilePicker.SourceFilePath = dlg.FileName;

				ApplicationContext.Instance.LoggerVM.AddLogLine("Source file specified. Can start crawling.");
			}
		}

		public void BindContext(FrameworkElement element)
		{
			element.DataContext = FilePicker;
		}
	}
}

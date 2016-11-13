using System.Windows.Input;
using Microsoft.Win32;
using WpfClient.Commands;

namespace WpfClient.ViewModel
{
	internal class SourceFilePickerViewModel : BaseViewModel
	{
		#region Fields
		private string sourceFilePath;
		private ButtonCommand chooseFileBtnClick;
		private OpenFileDialog fileDialog;
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

					RaisePropertyChanged(nameof(SourceFilePath));
				}
			}
		}

		public ICommand ChooseFileBtnClick
		{
			get
			{
				return chooseFileBtnClick;
			}
		}
		#endregion

		public SourceFilePickerViewModel()
			: base(ViewModelId.SourceFilePicker)
		{
			chooseFileBtnClick = new ButtonCommand(ChooseFileFromDialog);

			SetupFileDialog();
		}

		private void SetupFileDialog()
		{
			fileDialog = new OpenFileDialog();

			fileDialog.DefaultExt = ".xml";
			fileDialog.Filter = "Xml Files (*.xml)|*.xml";
		}

		public void ChooseFileFromDialog(object param)
		{
			if (fileDialog.ShowDialog() != null)
			{
				SourceFilePath = fileDialog.FileName;

				LoggerViewModel.Instance.LogMessage("Source file defined.");
				ViewModelsMediator.Instance.OnSourceFileChosen(SourceFilePath);
			}
		}
	}
}

using System.Windows;
using WpfClient.ViewModel;

namespace WpfClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			ApplicationContext.CreateContext();

			InitializeComponent();
		}

		private void OnSourceFilePickerLoaded(object sender, RoutedEventArgs e)
		{
			var sourceFileVM = ApplicationContext.Instance.SourceFileVM;

			sourceFileVM.BindContext(FilePickerTextBox);
			sourceFileVM.BindContext(FilePickerButton);
		}

		private void OnStatusBarLoaded(object sender, RoutedEventArgs e)
		{
			var statusViewModel = ApplicationContext.Instance.StatusVM;
			statusViewModel.SetStatus("Ready", Model.StatusBarModel.MessageType.Info);

			statusViewModel.BindContext(statusBar);
		}

		private void OnChooseSourceFile(object sender, RoutedEventArgs e)
		{
			var sourceFileViewModel = ApplicationContext.Instance.SourceFileVM;

			sourceFileViewModel.ChooseFileFromDialog();
		}

		private void OnWindowLoaded(object sender, RoutedEventArgs e)
		{
			InitCrawlerTreeViewModel();
		}

		private void InitCrawlerTreeViewModel()
		{
			var crawlerViewModel = ApplicationContext.Instance.CrawlerTreeVM;

			crawlerViewModel.BindContext(StartBtn);
			crawlerViewModel.BindContext(StopBtn);
		}

		private void OnStartCrawling(object sender, RoutedEventArgs e)
		{
			ApplicationContext.Instance.CrawlerTreeVM.OnStartCrawling();
		}

		private void OnLogWindowLoaded(object sender, RoutedEventArgs e)
		{
			ApplicationContext.Instance.LoggerVM.BindContext(LogWindow);
		}
	}
}

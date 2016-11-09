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
			InitializeComponent();
		}

		private void OnStatusBarLoaded(object sender, RoutedEventArgs e)
		{
			StatusBarViewModel statusBarViewModel = new StatusBarViewModel();
			statusBarViewModel.SetStatus("Ready", Model.StatusBarModel.MessageType.Info);

			statusBar.DataContext = statusBarViewModel.StatusBar;
		}
	}
}

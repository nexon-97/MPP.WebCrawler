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

		private void CrawlingOutputTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			ViewModelsMediator.Instance.OnCrawlerTreeViewSelectionChanged(e.NewValue as CrawlerTreeViewItem);
		}
	}
}

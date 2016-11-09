using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfClient.Model;

namespace WpfClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public StatusBarModel StatusBar { get; set; }

		public MainWindow()
		{
			StatusBar = new StatusBarModel();

			InitializeComponent();
			DataContext = this;
		}

		private void OnStatusBarLoaded(object sender, RoutedEventArgs e)
		{
			StatusBar.StatusText = "Ready.";
			StatusBar.BackgroundColor = new SolidColorBrush(Color.FromRgb(200, 255, 180));
			StatusBar.TextColor = new SolidColorBrush(Colors.Black);
		}
	}
}

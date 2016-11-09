using System.Windows;

namespace WpfClient.ViewModel
{
	interface IViewModel
	{
		void BindContext(FrameworkElement element);
	}
}

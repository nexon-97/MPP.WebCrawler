using WpfClient.Model;
using System.Collections.ObjectModel;

namespace WpfClient.ViewModel
{
	public class StatusBarViewModel
	{
		public ObservableCollection<StatusBarModel> StatusBars
		{
			get;
			set;
		}

		public void LoadStatusBars()
		{
			ObservableCollection<StatusBarModel> statusBars = new ObservableCollection<StatusBarModel>();

			statusBars.Add(new StatusBarModel { StatusText = "Debug text" });

			StatusBars = statusBars;
		}
	}
}
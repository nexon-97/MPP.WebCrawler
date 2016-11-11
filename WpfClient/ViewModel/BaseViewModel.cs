using System.ComponentModel;

namespace WpfClient.ViewModel
{
	internal class BaseViewModel : INotifyPropertyChanged
	{
		#region Fields
		protected ViewModelId ViewModelDef { get; }
		#endregion

		#region INotifyPropertyChanged interface
		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string property)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}
		#endregion

		#region Constructor
		protected BaseViewModel(ViewModelId id)
		{
			ViewModelDef = id;
			ViewModelsMediator.Instance.RegisterViewModel(ViewModelDef, this);
		}
		#endregion
	}
}

namespace WpfClient.ViewModel
{
	public class MainViewModel : BaseViewModel
	{
		private string sourceFilePath;

		public string SourceFilePath
		{
			get
			{
				return "Idi nahui";
			}
			set
			{
				sourceFilePath = value;
			}
		}
	}
}

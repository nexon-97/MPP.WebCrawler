using WpfClient.ViewModel;

namespace WpfClient
{
	public class ApplicationContext
	{
		#region Fields
		// ViewModels
		private CrawlerTreeViewModel crawlerTreeVM;
		private StatusBarViewModel statusVM;
		private SourceFilePickerViewModel sourceFileVM;
		private LoggerViewModel loggerVM;
		#endregion

		#region Properties
		public CrawlerTreeViewModel CrawlerTreeVM
		{
			get
			{
				return crawlerTreeVM;
			}
		}

		public StatusBarViewModel StatusVM
		{
			get
			{
				return statusVM;
			}
		}

		public SourceFilePickerViewModel SourceFileVM
		{
			get
			{
				return sourceFileVM;
			}
		}

		public LoggerViewModel LoggerVM
		{
			get
			{
				return loggerVM;
			}
		}
		#endregion

		#region Singleton
		private static object contextLock = new object();
		private static ApplicationContext instance;

		public static ApplicationContext Instance
		{
			get
			{
				lock (contextLock)
				{
					return instance;
				}
			}
		}

		public static void CreateContext()
		{
			instance = new ApplicationContext();
		}
		#endregion

		private ApplicationContext()
		{
			// Init view models
			statusVM = new StatusBarViewModel();
			sourceFileVM = new SourceFilePickerViewModel();
			crawlerTreeVM = new CrawlerTreeViewModel();
			loggerVM = new LoggerViewModel();
		}
	}
}

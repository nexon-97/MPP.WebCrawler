using System.Collections.Generic;

namespace WpfClient.ViewModel
{
	internal class ViewModelsMediator
	{
		private Dictionary<ViewModelId, BaseViewModel> viewModels;

		public string SourceFilePath
		{
			get
			{
				var filePickerViewModel = viewModels[ViewModelId.SourceFilePicker] as SourceFilePickerViewModel;
				return filePickerViewModel.SourceFilePath;
			}
		}

		#region Singleton pattern
		private static ViewModelsMediator instance = new ViewModelsMediator();
		private static object instanceLock = new object();

		public static ViewModelsMediator Instance
		{
			get
			{
				lock (instanceLock)
				{
					return instance;
				}
			}
		}
		#endregion

		private ViewModelsMediator()
		{
			viewModels = new Dictionary<ViewModelId, BaseViewModel>();
		}

		public void RegisterViewModel(ViewModelId id, BaseViewModel viewModel)
		{
			viewModels.Add(id, viewModel);
		}

		public void OnSourceFileChosen(string path)
		{
			var crawlerTreeViewModel = viewModels[ViewModelId.CrawlerTree] as CrawlerTreeViewModel;

			crawlerTreeViewModel.ValidateSourcePath(path);
		}

		public void OnCrawlerTreeViewSelectionChanged(CrawlerTreeViewItem selection)
		{
			var resourceDescViewModel = viewModels[ViewModelId.ResourceDesc] as ResourceDescriptionViewModel;
			resourceDescViewModel.SetCurrentCrawlerNode(selection);
		}
	}
}

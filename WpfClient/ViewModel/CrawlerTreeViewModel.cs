using System;
using System.IO;
using System.Collections.Generic;
using WebCrawler;
using WpfClient.Commands;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace WpfClient.ViewModel
{
	using TreeViewItems = ObservableCollection<CrawlerTreeViewItem>;

	internal class CrawlerTreeViewModel : BaseViewModel
	{
		#region Fields
		private bool startBtnEnabled;
		private bool stopBtnEnabled;
		private int crawlingDepth;
		private TreeViewItems crawlerOutput;
		private CrawlerTreeViewItem selectedItem;
		#endregion

		#region Properties
		public bool StartBtnEnabled
		{
			get
			{
				return startBtnEnabled;
			}
			set
			{
				startBtnEnabled = value;
				RaisePropertyChanged("StartBtnEnabled");
			}
		}

		public bool StopBtnEnabled
		{
			get
			{
				return stopBtnEnabled;
			}
			set
			{
				stopBtnEnabled = value;
				RaisePropertyChanged("StopBtnEnabled");
			}
		}

		public TreeViewItems CrawlerOutput
		{
			get
			{
				return crawlerOutput;
			}
		}

		public string CrawlingDepth
		{
			get
			{
				return crawlingDepth.ToString();
			}
			set
			{
				int valueOld = crawlingDepth;
				if (!int.TryParse(value, out crawlingDepth))
				{
					crawlingDepth = valueOld;
				}

				RaisePropertyChanged("CrawlingDepth");
			}
		}

		public ButtonCommandAsync StartBtnClick
		{
			get; private set;
		}

		public ButtonCommand StopBtnClick
		{
			get; private set;
		}
		#endregion

		public CrawlerTreeViewModel()
			: base(ViewModelId.CrawlerTree)
		{
			crawlingDepth = 2;
			crawlerOutput = new TreeViewItems();

			StartBtnEnabled = false;
			StopBtnEnabled = false;

			StartBtnClick = new ButtonCommandAsync(OnStartCrawling);
			StopBtnClick = new ButtonCommand(OnStopCrawling);
		}

		public void ValidateSourcePath(string path)
		{
			StartBtnEnabled = File.Exists(path);
		}	

		public async Task OnStartCrawling(object param)
		{
			CrawlerInputParser inputParser = new CrawlerInputParser();
			List<Uri> rootResources = inputParser.Parse(ViewModelsMediator.Instance.SourceFilePath);

			if (rootResources != null)
			{
				//StartBtnEnabled = false;
				//StopBtnEnabled = true;

				// Pass control to webcrawler lib
				WebCrawler.WebCrawler crawler = new WebCrawler.WebCrawler();
				crawler.MaxDepth = crawlingDepth;
				crawler.Logger = LoggerViewModel.Instance;

				foreach (var rootUri in rootResources)
				{
					WebCrawlerOutput crawlerOutput = await crawler.PerformCrawlingAsync(rootUri, 0, -1);

					AppendCrawlerOutput(crawlerOutput, null);
				}

				//ApplicationContext.Instance.LoggerVM.AddLogLine("Crawling finished.");
			}
		}

		public void OnStopCrawling(object param)
		{
			
		}

		public CrawlerTreeViewItem AddCrawlerOutputTreeNode(CrawlerTreeViewItem parent, string item, WebCrawlerOutput attachment)
		{
			CrawlerTreeViewItem element = new CrawlerTreeViewItem();
			element.Header = item;
			element.AttachedData = attachment;

			if (parent != null)
			{
				parent.Items.Add(element);
			}
			else
			{
				CrawlerOutput.Add(element);
			}

			RaisePropertyChanged("CrawlerOutput");
			return element;
		}

		public void AppendCrawlerOutput(WebCrawlerOutput output, CrawlerTreeViewItem treeItem)
		{
			var element = AddCrawlerOutputTreeNode(treeItem, string.Format("ID {0}", output.SourceId), output);

			foreach (var child in output.Children)
			{
				AppendCrawlerOutput(child, element);
			}
		}
	}
}

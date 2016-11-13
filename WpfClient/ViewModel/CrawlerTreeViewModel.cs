using System;
using System.IO;
using System.Collections.Generic;
using WebCrawler;
using WpfClient.Commands;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace WpfClient.ViewModel
{
	using TreeViewItems = ObservableCollection<CrawlerTreeViewItem>;

	internal class CrawlerTreeViewModel : BaseViewModel
	{
		private const int MaxCrawlingDepth = 6;

		#region Fields
		private bool startBtnEnabled;
		private bool stopBtnEnabled;
		private int crawlingDepth;
		private TreeViewItems crawlerOutput;
		private Dictionary<int, CrawlerTreeViewItem> treeNodesDict;
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
				RaisePropertyChanged(nameof(StartBtnEnabled));
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
				RaisePropertyChanged(nameof(StopBtnEnabled));
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
				else
				{
					crawlingDepth = Utils.Clamp<int>(crawlingDepth, 1, MaxCrawlingDepth);
				}

				RaisePropertyChanged(nameof(CrawlingDepth));
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

			// Configure buttons state
			StartBtnEnabled = false;
			StopBtnEnabled = false;

			// Register commands
			StartBtnClick = new ButtonCommandAsync(OnStartCrawling);
			StopBtnClick = new ButtonCommand(OnStopCrawling);

			// Init internal tree view items quick access storage
			treeNodesDict = new Dictionary<int, CrawlerTreeViewItem>();
			//treeViewMutex = new Mutex();
		}

		public void ValidateSourcePath(string path)
		{
			StartBtnEnabled = File.Exists(path);
		}	

		public async Task OnStartCrawling(object param)
		{
			ClearCrawlerTree();

			CrawlerInputParser inputParser = new CrawlerInputParser();
			List<Uri> rootResources = inputParser.Parse(ViewModelsMediator.Instance.SourceFilePath);

			if (rootResources != null)
			{
				StartBtnEnabled = false;
				StopBtnEnabled = true;

				// Init crawler
				WebCrawler.WebCrawler crawler = new WebCrawler.WebCrawler();
				crawler.MaxDepth = crawlingDepth;
				crawler.Logger = LoggerViewModel.Instance;
				crawler.LoadingFinished += AddCrawlerElement;

				foreach (var rootUri in rootResources)
				{
					WebCrawlerOutput crawlerOutput = await crawler.PerformCrawlingAsync(rootUri, 0, -1);
				}

				StartBtnEnabled = true;
				StopBtnEnabled = false;
			}
		}

		public void OnStopCrawling(object param)
		{

		}

		public CrawlerTreeViewItem AddCrawlerOutputTreeNode(CrawlerTreeViewItem parent, string item, WebCrawlerOutput attachment)
		{
			// Construct element
			CrawlerTreeViewItem element = new CrawlerTreeViewItem();
			element.Header = item;
			element.AttachedData = attachment;

			// Attach to the parent
			if (parent != null)
			{
				parent.Items.Add(element);
			}
			else
			{
				CrawlerOutput.Add(element);
			}

			treeNodesDict.Add(attachment.SourceId, element); // Register for quick access
			RaisePropertyChanged(nameof(CrawlerOutput));

			return element;
		}

		public void AppendCrawlerOutput(WebCrawlerOutput output, CrawlerTreeViewItem treeItem)
		{
			string elementName = string.Format("ID {0}", output.SourceId);
			AddCrawlerOutputTreeNode(treeItem, elementName, output);
		}

		private void AddCrawlerElement(int parentId, WebCrawlerOutput output)
		{
			LoggerViewModel.Instance.LogMessage(string.Format("Resource loaded: {0}", output.SourceUri));

			var parentItem = FindTreeItemById(parentId);
			AppendCrawlerOutput(output, parentItem);
		}

		private CrawlerTreeViewItem FindTreeItemById(int id)
		{
			return treeNodesDict.ContainsKey(id) ? treeNodesDict[id] : null;
		}

		private void ClearCrawlerTree()
		{
			CrawlerOutput.Clear();
			treeNodesDict.Clear();

			RaisePropertyChanged(nameof(CrawlerOutput));

			LoggerViewModel.Instance.LogMessage("Output cleared.");
		}
	}
}

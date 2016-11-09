using WpfClient.Model;
using System;
using System.Windows;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebCrawler;

namespace WpfClient.ViewModel
{
	public class CrawlerTreeViewModel : IViewModel
	{
		public CrawlerTreeModel CrawlerTree { get; set; }

		public CrawlerTreeViewModel()
		{
			CrawlerTree = new CrawlerTreeModel();
			CrawlerTree.StartBtnEnabled = false;
			CrawlerTree.StopBtnEnabled = false;
		}

		public void BindContext(FrameworkElement element)
		{
			element.DataContext = CrawlerTree;
		}

		public void ValidateSourcePath(string path)
		{
			CrawlerTree.StartBtnEnabled = File.Exists(path);
		}

		private XmlElement ParseXmlSource(out int depth)
		{
			var logger = ApplicationContext.Instance.LoggerVM;
			XmlDocument document = new XmlDocument();

			depth = 0;
			try
			{
				document.Load(ApplicationContext.Instance.SourceFileVM.SourceFilePath);
			}
			catch (Exception)
			{
				logger.AddLogLine("Failed to parse source file!");
				return null;
			}

			logger.AddLogLine("Source file parsed.");

			var rootElement = document.FirstChild;

			XmlElement resourcesNode = null;
			if (rootElement != null)
			{
				foreach (var child in rootElement)
				{
					var element = child as XmlElement;
					const string DepthNode = "depth";
					const string RootResourcesNode = "rootResources";

					if (element.Name.Equals(DepthNode))
					{
						if (!int.TryParse(element.InnerText, out depth))
						{
							logger.AddLogLine("Can't parse crawling depth! Setting default value of 2.");
							depth = 2;
						}
					}
					else if (element.Name.Equals(RootResourcesNode))
					{
						resourcesNode = element;
					}
				}
			}

			return resourcesNode;
		}

		private List<Uri> ParseCrawlerInput(out int depth)
		{
			XmlElement resourcesContainerNode = ParseXmlSource(out depth);

			if (resourcesContainerNode != null)
			{
				List<Uri> rootResourcesList = new List<Uri>();
				var logger = ApplicationContext.Instance.LoggerVM;

				foreach (var resource in resourcesContainerNode)
				{
					var resourceNode = resource as XmlElement;

					try
					{
						Uri resourceUri = new Uri(resourceNode.InnerText);
						rootResourcesList.Add(resourceUri);
					}
					catch (UriFormatException)
					{
						logger.AddLogLine(string.Format("Invalid URI: {0}. Skipped.", resourceNode.InnerText));
					}
					catch (ArgumentNullException)
					{
						logger.AddLogLine("Empty resource. Skipped.");
					}
				}

				return rootResourcesList;
			}

			return null;
		}

		public void OnStartCrawling()
		{
			int crawlingDepth;
			List<Uri> rootResources = ParseCrawlerInput(out crawlingDepth);

			if (rootResources != null)
			{
				ApplicationContext.Instance.LoggerVM.AddLogLine("Attaching webcrawler lib.");

				CrawlerTree.StartBtnEnabled = false;
				CrawlerTree.StopBtnEnabled = true;

				// Pass control to webcrawler lib
				WebCrawler.WebCrawler crawler = new WebCrawler.WebCrawler();
				crawler.MaxDepth = crawlingDepth;

				foreach (var rootUri in rootResources)
				{
					Task<WebCrawlerOutput> crawlerOutput = crawler.PerformCrawlingAsync(rootUri, 0);

					ApplicationContext.Instance.LoggerVM.AddLogLine(crawlerOutput.Result.Print(0));
				}

				ApplicationContext.Instance.LoggerVM.AddLogLine("Crawling finished.");
				//ApplicationContext.Instance.LoggerVM.AddLogLine("Result:");


				
			}
		}
	}
}

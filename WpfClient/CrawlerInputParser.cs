using System;
using System.Collections.Generic;
using System.Xml;
using WpfClient.ViewModel;

namespace WpfClient
{
	internal class CrawlerInputParser
	{
		public List<Uri> Parse(string path)
		{
			XmlElement resourcesContainerNode = ParseXmlSource(path);

			if (resourcesContainerNode != null)
			{
				List<Uri> rootResourcesList = new List<Uri>();
				var logger = LoggerViewModel.Instance;

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
						logger.LogMessage(string.Format("Invalid URI: {0}. Skipped.", resourceNode.InnerText));
					}
					catch (ArgumentNullException)
					{
						logger.LogMessage("Empty resource. Skipped.");
					}
				}

				return rootResourcesList;
			}

			return null;
		}

		private XmlElement ParseXmlSource(string path)
		{
			var logger = LoggerViewModel.Instance;
			XmlDocument document = new XmlDocument();

			try
			{
				document.Load(path);
			}
			catch (Exception)
			{
				logger.LogMessage("Failed to parse source file!");
				return null;
			}

			logger.LogMessage("Source file parsed.");

			var rootElement = document.FirstChild;

			XmlElement resourcesNode = null;
			if (rootElement != null)
			{
				foreach (var child in rootElement)
				{
					var element = child as XmlElement;
					const string RootResourcesNode = "rootResources";

					if (element.Name.Equals(RootResourcesNode))
					{
						resourcesNode = element;
					}
				}
			}

			return resourcesNode;
		}
	}
}

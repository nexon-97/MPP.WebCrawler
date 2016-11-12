using System.Windows.Controls;
using WebCrawler;

namespace WpfClient
{
	internal class CrawlerTreeViewItem : TreeViewItem
	{
		public WebCrawlerOutput AttachedData { get; set; }
	}
}

using System.Windows.Controls;
using WebCrawler;

namespace WpfClient
{
	public class CrawlerTreeViewItem : TreeViewItem
	{
		public WebCrawlerOutput AttachedData { get; set; }
	}
}

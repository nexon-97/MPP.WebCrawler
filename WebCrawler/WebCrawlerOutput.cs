using System.Collections.Generic;
using System.Text;

namespace WebCrawler
{
	public class WebCrawlerOutput
	{
		public int SourceId { get; set; }
		public List<WebCrawlerOutput> Children { get; }

		public WebCrawlerOutput(int sourceId)
		{
			SourceId = sourceId;
			Children = new List<WebCrawlerOutput>();
		}

		public void AddChild(WebCrawlerOutput child)
		{
			Children.Add(child);
		}

		public string Print(int tab)
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendFormat("{0}-> (url{1})\n", GetTabs(tab), SourceId);

			foreach (var item in Children)
			{
				builder.Append(item.Print(tab + 1));
			}

			return builder.ToString();
		}

		private string GetTabs(int tab)
		{
			return new string(' ', tab * 3);
		}
	}
}

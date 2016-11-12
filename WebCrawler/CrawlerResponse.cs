using System.Net;

namespace WebCrawler
{
	internal struct CrawlerResponse
	{
		public byte[] Content { get; set; }
		public WebResponse Response { get; set; }
	}
}

using System.Net;
using System.Text;

namespace WebCrawler
{
	public struct CrawlerResponse
	{
		public byte[] content;
		public Encoding contentEncoding;
		public WebResponse response;
	}
}

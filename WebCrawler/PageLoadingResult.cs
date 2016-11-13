using System.Net;

namespace WebCrawler
{
	internal struct PageLoadingResult
	{
		public byte[] Content { get; set; }
		public WebResponse Response { get; set; }
	}
}

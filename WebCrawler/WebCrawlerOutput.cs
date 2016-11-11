using System;
using System.Collections.Generic;
using System.Net;

namespace WebCrawler
{
	public class WebCrawlerOutput
	{
		public int SourceId { get; private set; }
		public Uri SourceUri { get; private set; }
		public byte[] Content { get; private set; }
		public WebResponse Response { get; private set; }
		public List<WebCrawlerOutput> Children { get; private set; }

		public WebCrawlerOutput(int sourceId, Uri sourceUri, WebResponse response, byte[] content)
		{
			SourceId = sourceId;
			SourceUri = sourceUri;
			Response = response;
			Content = content;

			Children = new List<WebCrawlerOutput>();
		}

		public void AddChild(WebCrawlerOutput child)
		{
			Children.Add(child);
		}
	}
}

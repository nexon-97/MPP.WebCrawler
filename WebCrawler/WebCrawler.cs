using System;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace WebCrawler
{
	public class WebCrawler
	{
		private const int NotFoundIndex = -1;
		private const int DefaultDepth = 3;

		#region Fields
		private List<Uri> uniqueUriList;
		#endregion

		#region Properties
		public int MaxDepth { get; set; }
		public ILogger Logger { get; set; }
		#endregion

		public WebCrawler()
		{
			MaxDepth = DefaultDepth;
			uniqueUriList = new List<Uri>();
		}

		public byte[] LoadHtmlPage(Uri uri, out WebResponse response)
		{
			WebRequest request = WebRequest.Create(uri);
			response = null;

			try
			{
				response = request.GetResponse();

				var stream = response.GetResponseStream();
				using (var reader = new StreamReader(stream))
				{
					string content = reader.ReadToEnd();
					return Encoding.ASCII.GetBytes(content);
				}
			}
			catch (WebException e)
			{
				LogMessage(string.Format("{0}: {1}", uri, e.Message));
			}

			return null;	
		}

		public int GetUriId(Uri uri)
		{
			return uniqueUriList.FindIndex(x => uri.AbsoluteUri.Equals(x.AbsoluteUri));
		}

		public int AddUri(Uri uri)
		{
			int uriId = GetUriId(uri);
			if (uriId == NotFoundIndex)
			{
				uniqueUriList.Add(uri);
				return uniqueUriList.Count - 1;
			}
			else
			{
				return uriId;
			}
		}

		public async Task<WebCrawlerOutput> PerformCrawlingAsync(Uri uri, int currentDepth, int parentId)
		{
			int uriId = GetUriId(uri);
			if (uriId == NotFoundIndex)
			{
				WebResponse response;
				byte[] pageContent = LoadHtmlPage(uri, out response);

				WebCrawlerOutput output = new WebCrawlerOutput(AddUri(uri), uri, response, pageContent);
				uriId = GetUriId(uri); // Update uri unique id
				
				if (pageContent != null)
				{
					LinkExtractor extractor = new LinkExtractor();
					List<Uri> childUris = extractor.ExtractLinksFromPage(uri, pageContent);

					currentDepth++;
					if (currentDepth < MaxDepth)
					{
						foreach (var item in childUris)
						{
							var childOutput = await PerformCrawlingAsync(item, currentDepth, uriId);
							if (childOutput != null)
							{
								output.AddChild(childOutput);
							}
						}
					}
				}
				
				return output;
			}

			return null;
		}

		public Uri GetUriById(int id)
		{
			if (id >= 0 && uniqueUriList.Count > id)
			{
				return uniqueUriList[id];
			}

			return null;
		}

		private void LogMessage(string message)
		{
			if (Logger != null)
			{
				Logger.LogMessage(message);
			}
		}
	}
}

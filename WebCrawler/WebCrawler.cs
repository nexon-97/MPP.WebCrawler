using System;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace WebCrawler
{
	public delegate void PageLoadingFinished(int parentId, WebCrawlerOutput output);

	public class WebCrawler
	{
		#region Constants
		private const int NotFoundIndex = -1;
		private const int DefaultDepth = 3;
		#endregion

		#region Fields
		private List<Uri> uniqueUriList;
		#endregion

		#region Properties
		public int MaxDepth { get; set; }
		public ILogger Logger { get; set; }
		#endregion

		public event PageLoadingFinished LoadingFinished;

		public WebCrawler()
		{
			MaxDepth = DefaultDepth;
			uniqueUriList = new List<Uri>();
		}

		internal async Task<CrawlerResponse> LoadHtmlPage(Uri uri)
		{
			CrawlerResponse output = new CrawlerResponse();
			WebRequest request = WebRequest.Create(uri);

			try
			{
				output.Response = await request.GetResponseAsync();

				var stream = output.Response.GetResponseStream();
				using (var reader = new StreamReader(stream))
				{
					output.Content = Encoding.ASCII.GetBytes(reader.ReadToEnd());
					return output;
				}
			}
			catch (WebException e)
			{
				LogMessage(string.Format("{0}: {1}", uri, e.Message));
			}

			return new CrawlerResponse();
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
				var loadingResult = await LoadHtmlPage(uri);
				
				WebCrawlerOutput output = new WebCrawlerOutput(
					AddUri(uri), uri, loadingResult.Response, loadingResult.Content);
				uriId = GetUriId(uri); // Update uri unique id

				if (LoadingFinished != null)
				{
					LoadingFinished(parentId, output);
				}

				if (loadingResult.Content != null)
				{
					LinkExtractor extractor = new LinkExtractor();
					List<Uri> childUris = extractor.ExtractLinksFromPage(uri, loadingResult.Content);

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

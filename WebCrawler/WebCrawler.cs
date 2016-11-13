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

		#region Events
		public event PageLoadingFinished LoadingFinished;
		#endregion

		#region Constructor
		public WebCrawler()
		{
			MaxDepth = DefaultDepth;
			uniqueUriList = new List<Uri>();
		}
		#endregion

		private int GetUriId(Uri uri)
		{
			return uniqueUriList.FindIndex(x => uri.AbsoluteUri.Equals(x.AbsoluteUri));
		}

		private int AddUri(Uri uri)
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

		private bool IsUriUnique(Uri uri)
		{
			return GetUriId(uri) == NotFoundIndex;
		}

		public async Task<WebCrawlerOutput> PerformCrawlingAsync(Uri uri, int currentDepth, int parentId)
		{
			if (IsUriUnique(uri))
			{
				// Load web resource
				var pageLoader = new WebPageLoader();
				var loadResult = await pageLoader.LoadAsync(uri);

				// Register resource
				int uriId = AddUri(uri);
				// Generate crawler output
				WebCrawlerOutput output = new WebCrawlerOutput(
					uriId, uri, loadResult.Response, loadResult.Content);

				NotifyResourceLoadingFinished(parentId, output);

				currentDepth++;
				if (loadResult.Content != null && currentDepth < MaxDepth)
				{
					ExtractPageLinks(loadResult.Content, uri, output, currentDepth);
				}

				return output;
			}

			return null;
		}

		private async void ExtractPageLinks(byte[] content, Uri parentUri, WebCrawlerOutput output, int depth)
		{
			LinkExtractor extractor = new LinkExtractor();
			Encoding responseEncoding = GetEncodingFromResponse(output.Response);
			List<Uri> childUris = extractor.ExtractLinksFromPage(parentUri, content, responseEncoding);

			foreach (var link in childUris)
			{
				var childOutput = await PerformCrawlingAsync(link, depth, GetUriId(parentUri));
				if (childOutput != null)
				{
					output.AddChild(childOutput);
				}
			}
		}

		private Encoding GetEncodingFromResponse(WebResponse response)
		{
			// Check for http response
			var httpResponse = response as HttpWebResponse;
			if (httpResponse != null)
			{
				return Encoding.GetEncoding(httpResponse.CharacterSet);	
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

		private void NotifyResourceLoadingFinished(int parentId, WebCrawlerOutput output)
		{
			if (LoadingFinished != null)
			{
				LoadingFinished(parentId, output);
			}
		}
	}
}

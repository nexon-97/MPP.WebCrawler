using System;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace WebCrawler
{
	public class WebCrawler
	{
		private const int NotFoundIndex = -1;
		private const int DefaultDepth = 5;

		#region Fields
		private List<Uri> _uniqueUriList;
		#endregion

		#region Properties
		public int MaxDepth { get; set; }
		#endregion

		public WebCrawler()
		{
			MaxDepth = DefaultDepth;
			_uniqueUriList = new List<Uri>();
		}

		public string LoadHtmlPage(Uri uri)
		{
			WebRequest request = WebRequest.Create(uri);

			try
			{
				WebResponse response = request.GetResponse();

				var stream = response.GetResponseStream();
				using (var reader = new StreamReader(stream))
				{
					return reader.ReadToEnd();
				}
			}
			catch (WebException e)
			{
				Console.WriteLine(e.Message);
			}

			return string.Empty;	
		}

		public int GetUriId(Uri uri)
		{
			return _uniqueUriList.FindIndex(x => uri.AbsoluteUri.Equals(x.AbsoluteUri));
		}

		public int AddUri(Uri uri)
		{
			int uriId = GetUriId(uri);
			if (uriId == NotFoundIndex)
			{
				_uniqueUriList.Add(uri);
				return _uniqueUriList.Count - 1;
			}
			else
			{
				return uriId;
			}
		}

		public async Task<WebCrawlerOutput> PerformCrawlingAsync(Uri uri, int currentDepth)
		{
			if (GetUriId(uri) == NotFoundIndex)
			{
				WebCrawlerOutput output = new WebCrawlerOutput(AddUri(uri));
				LinkExtractor extractor = new LinkExtractor();

				string pageContent = LoadHtmlPage(uri);
				List<Uri> childUris = extractor.ExtractLinksFromPage(uri, pageContent);

				currentDepth++;
				if (currentDepth < MaxDepth)
				{
					foreach (var item in childUris)
					{
						var childOutput = await PerformCrawlingAsync(item, currentDepth);
						if (childOutput != null)
						{
							output.AddChild(childOutput);
						}
					}
				}

				return output;
			}

			return null;
		}
	}
}

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace WebCrawler
{
	internal class WebPageLoader
	{
		public ILogger Logger { get; set; }

		internal async Task<PageLoadingResult> LoadAsync(Uri uri)
		{
			PageLoadingResult result = new PageLoadingResult();
			var request = WebRequest.Create(uri);

			try
			{
				result.Response = await request.GetResponseAsync();

				var stream = result.Response.GetResponseStream();
				using (var reader = new BinaryReader(stream))
				{
					result.Content = reader.ReadBytes((int)result.Response.ContentLength);
					return result;
				}
			}
			catch (WebException e)
			{
				LogMessage(string.Format("{0}: {1}", uri, e.Message));
			}

			return new PageLoadingResult();
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

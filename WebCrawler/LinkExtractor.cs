using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WebCrawler
{
	internal class LinkExtractor
	{
		private const int NotFoundIndex = -1;
		private const string SearchPattern = "<a ";
		private const string TagEndingPattern = ">";
		private const string AbsolutePathFeature = "://";

		public List<Uri> ExtractLinksFromPage(Uri uri, byte[] content)
		{
			string downloadedPage = Encoding.ASCII.GetString(content);

			List<Uri> links = new List<Uri>();
			int searchFromIndex = 0;

			while (searchFromIndex < downloadedPage.Length)
			{
				// Try find <a> tag beginning
				int linkNodeIndex = downloadedPage.IndexOf(SearchPattern, searchFromIndex);
				if (linkNodeIndex == NotFoundIndex)
				{
					break;
				}

				// Try find tag ending
				int endingIndex = downloadedPage.IndexOf(TagEndingPattern, linkNodeIndex + 1);
				if (endingIndex != NotFoundIndex)
				{
					string tagHeader = downloadedPage.Substring(linkNodeIndex, endingIndex - linkNodeIndex + 1);

					Regex hrefPattern = new Regex("\\shref=[\"']([^\\s]+)[\"']");
					Match match = hrefPattern.Match(tagHeader);

					if (match.Success)
					{
						string href = match.Groups[1].Value;
						links.Add(ClearUri(ConstructUri(uri, href)));
					}

					searchFromIndex = endingIndex + 1;
					continue;
				}

				searchFromIndex = linkNodeIndex + 1;
			}

			return links;
		}

		public Uri ConstructUri(Uri source, string href)
		{
			bool isAbsolute = href.Contains(AbsolutePathFeature);
			if (isAbsolute)
			{
				return new Uri(href);
			}
			else
			{
				int lastSeparator = source.AbsoluteUri.LastIndexOf('/');
				string truncatedPath = source.AbsoluteUri.Substring(0, lastSeparator + 1);
				return new Uri(truncatedPath + href);
			}
		}

		private Uri ClearUri(Uri uri)
		{
			string absoluteUri = uri.AbsoluteUri;

			// Remove anchors
			int anchorIndex = absoluteUri.IndexOf('#');
			if (anchorIndex != NotFoundIndex)
			{
				absoluteUri = absoluteUri.Substring(0, anchorIndex);
			}

			// Remove duplicate slashes
			const string DuplicateSlashesPattern = @"([^:/])/{2,}";
			absoluteUri = Regex.Replace(absoluteUri, DuplicateSlashesPattern, m => m.Groups[1].Value + "/");

			return new Uri(absoluteUri);
		}
	}
}

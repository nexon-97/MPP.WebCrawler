using System.Net;
using System.Windows;
using System.IO;
using WpfClient.Commands;
using Microsoft.Win32;
using System;

namespace WpfClient.ViewModel
{
	internal class ResourceDescriptionViewModel : BaseViewModel
	{
		#region Fields
		private string resourceId;
		private string resourceUri;
		private string responseText;
		private string contentDesc;
		private Visibility linkDescVisible;
		public bool saveContentBtnEnabled;
		private SaveFileDialog fileDialog;
		private byte[] content;
		#endregion

		#region Properties
		public string ResourceId
		{
			get
			{
				return resourceId;
			}
			set
			{
				resourceId = value;

				RaisePropertyChanged("ResourceId");
			}
		}

		public string ResourceUri
		{
			get
			{
				return resourceUri;
			}
			set
			{
				resourceUri = value;

				RaisePropertyChanged("ResourceUri");
			}
		}

		public string ResponseText
		{
			get
			{
				return responseText;
			}
			set
			{
				responseText = value;

				RaisePropertyChanged("ResponseText");
			}
		}

		public string ContentDesc
		{
			get
			{
				return contentDesc;
			}
			set
			{
				contentDesc = value;

				RaisePropertyChanged("ContentDesc");
			}
		}

		public Visibility LinkDescVisible
		{
			get
			{
				return linkDescVisible;
			}
			set
			{
				linkDescVisible = value;

				RaisePropertyChanged("LinkDescVisible");
			}
		}

		public bool SaveContentBtnEnabled
		{
			get
			{
				return saveContentBtnEnabled;
			}
			set
			{
				saveContentBtnEnabled = value;

				RaisePropertyChanged("SaveContentBtnEnabled");
			}
		}

		public ButtonCommand SaveToFileCommand
		{
			get; private set;
		}
		#endregion

		public ResourceDescriptionViewModel()
			: base(ViewModelId.ResourceDesc)
		{
			SaveToFileCommand = new ButtonCommand(OnSaveToFile);

			fileDialog = new SaveFileDialog();
			fileDialog.Filter = "Any file|*.*";

			LinkDescVisible = Visibility.Hidden;
		}

		public void SetCurrentCrawlerNode(CrawlerTreeViewItem node)
		{
			if (node != null)
			{
				LinkDescVisible = Visibility.Visible;

				var data = node.AttachedData;
				ResourceId = data.SourceId.ToString();
				ResourceUri = data.SourceUri.ToString();

				var response = data.Response;
				bool hasResponse = (response != null);
				bool hasContent = (hasResponse && response.ContentLength > 0);

				SaveContentBtnEnabled = hasContent;
				if (hasContent)
				{
					ContentDesc = string.Format("{0}, {1} {2}", response.ContentType, data.Content.Length, "B");
					content = data.Content;
				}
				else
				{
					ContentDesc = "No content";
					content = null;
				}

				if (hasResponse)
				{
					var httpResponse = response as HttpWebResponse;
					if (httpResponse != null)
					{
						ResponseText = httpResponse.StatusCode.ToString();
					}
					else
					{
						ResponseText = "General response";
					}
				}
				else
				{
					ResponseText = "No response";
				}
			}
			else
			{
				LinkDescVisible = Visibility.Hidden;
			}
		}

		private void OnSaveToFile(object param)
		{
			var filename = resourceUri;
			int paramsBegin = filename.IndexOf("?");
			if (paramsBegin != -1)
			{
				filename = filename.Substring(0, paramsBegin);
			}

			int pathEnd = filename.LastIndexOf("/");
			if (pathEnd != -1)
			{
				filename = filename.Substring(pathEnd + 1);
			}

			fileDialog.FileName = filename;

			if (fileDialog.ShowDialog() == true)
			{
				File.WriteAllBytes(fileDialog.FileName, content);
			}
		}
	}
}

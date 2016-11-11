using System.Windows;

namespace WpfClient.ViewModel
{
	public class ResourceDescriptionViewModel : BaseViewModel
	{
		#region Fields
		private string resourceId;
		private string resourceUri;
		private string responseText;
		private Visibility linkDescVisible;
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
		#endregion

		public ResourceDescriptionViewModel()
		{
			LinkDescVisible = Visibility.Collapsed;
		}

		public void SetCurrentCrawlerNode(CrawlerTreeViewItem node)
		{
			if (node != null)
			{
				LinkDescVisible = Visibility.Visible;

				var data = node.AttachedData;
				ResourceId = data.SourceId.ToString();
				ResourceUri = data.SourceUri.ToString();
				ResponseText = "404 ERROR!";
			}
			else
			{
				LinkDescVisible = Visibility.Collapsed;
			}
		}
	}
}

using LexiconLMS.Shared.Entities;

namespace LexiconLMS.Client.Components
{
	public partial class DocumentList
	{

		public string Message { get; set; } = string.Empty;

		private void DownloadDocument(Document document)
		{
			Message = "Document Downloaded";
		}
	}
}
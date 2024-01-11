using LexiconLMS.Shared.Entities;

namespace LexiconLMS.Client.Components

	
{
	public partial class ModulesList
	{

		private string GetRowStyle(Module item)
		{

			if (DateTime.Now > item.StartDate && DateTime.Now < item.EndDate)
			{
				return "background-color: lightgreen;";
			}

			return string.Empty;
		}
	}
}

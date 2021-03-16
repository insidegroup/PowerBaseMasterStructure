using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
	public class OptionalFieldItemsVM : CWTBaseViewModel
	{
		public OptionalFieldGroup OptionalFieldGroup { get; set; }
		public CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFieldGroupOptionalFieldItems_v1Result> OptionalFieldItems { get; set; }
		public bool HasWriteAccess { get; set; }
		public bool CanCreate { get; set; }
		public bool CanEditOrder { get; set; }

		public OptionalFieldItemsVM()
		{
			HasWriteAccess = false;
			CanCreate = false;
			CanEditOrder = false;
		}

		public OptionalFieldItemsVM(OptionalFieldGroup optionalFieldGroup, 
									CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFieldGroupOptionalFieldItems_v1Result> optionalFieldItems,
									bool hasWriteAccess,
									bool canCreate,
									bool canEditOrder)
		{
			OptionalFieldGroup = optionalFieldGroup;
			OptionalFieldItems = optionalFieldItems;
			HasWriteAccess = hasWriteAccess;
			CanCreate = canCreate;
			CanEditOrder = canEditOrder;
		}
	}
}
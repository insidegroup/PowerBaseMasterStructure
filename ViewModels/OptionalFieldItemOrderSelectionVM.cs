using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Include = "OptionalFieldGroup, ProductId")]
	public class OptionalFieldItemOrderSelectionVM : CWTBaseViewModel
	{
		public OptionalFieldGroup OptionalFieldGroup { get; set; }

		[Required(ErrorMessage = "You must choose a Product")]
		public int ProductId { get; set; }

		public OptionalFieldItemOrderSelectionVM()
		{
		}

		public OptionalFieldItemOrderSelectionVM(int productId, OptionalFieldGroup optionalFieldGroup)
		{
			OptionalFieldGroup = optionalFieldGroup;
			ProductId = productId;
		}
	}
}
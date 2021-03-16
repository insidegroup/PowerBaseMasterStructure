using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.ViewModels
{

	[Bind(Include = "ServicingOptionGroup, ServicingOptions, ServicingOptionId")]
	public class ServicingOptionHFLFVM : CWTBaseViewModel
	{
		public ServicingOptionGroup ServicingOptionGroup { get; set; }
		public IEnumerable<SelectListItem> ServicingOptions { get; set; }

		[Required(ErrorMessage = "You must choose a Servicing Option")]
		public int ServicingOptionId { get; set; }

		public ServicingOptionHFLFVM()
		{
		}
		public ServicingOptionHFLFVM(IEnumerable<SelectListItem> servicingOptions, int servicingOptionId, ServicingOptionGroup servicingOptionGroup)
		{
			ServicingOptionId = servicingOptionId;
			ServicingOptions = servicingOptions;
			ServicingOptionGroup = servicingOptionGroup;
		}
	}
}
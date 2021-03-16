using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	public class OptionalFieldLookupValueVM : CWTBaseViewModel
	{
		public OptionalFieldLookupValue OptionalFieldLookupValue { get; set; }
		public OptionalFieldLookupValueLanguage OptionalFieldLookupValueLanguage { get; set; }
	    public IEnumerable<SelectListItem> OptionalFieldLookupValueLanguages { get; set; }

	    public OptionalFieldLookupValueVM()
        {
        }

		public OptionalFieldLookupValueVM(OptionalFieldLookupValue optionalFieldLookupValue)
        {
			OptionalFieldLookupValue = optionalFieldLookupValue;
        }
    }
}
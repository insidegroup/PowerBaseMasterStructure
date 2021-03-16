using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class OptionalFieldLanguageVM : CWTBaseViewModel
    {
		public OptionalFieldLanguage OptionalFieldLanguage { get; set; }
	    public IEnumerable<SelectListItem> OptionalFieldLanguages { get; set; }

	    public OptionalFieldLanguageVM()
        {
        }

		public OptionalFieldLanguageVM(OptionalFieldLanguage optionalFieldLanguage)
        {
			OptionalFieldLanguage = optionalFieldLanguage;
        }
    }
}
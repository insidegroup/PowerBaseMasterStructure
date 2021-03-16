using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class OptionalFieldVM : CWTBaseViewModel
    {
        public OptionalField OptionalField { get; set; }
        public IEnumerable<SelectListItem> OptionalFieldStyles { get; set; }

        public OptionalFieldVM()
        {
        }

        public OptionalFieldVM(OptionalField optionalField)
        {
            OptionalField = optionalField;
        }
    }
}
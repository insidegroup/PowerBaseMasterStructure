using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ProductReasonItemOrderTypeSelectionVMValidation))]
    public class ProductReasonItemOrderTypeSelectionVM : CWTDesktopDatabase.ViewModels.CWTBaseViewModel
    {
        public IEnumerable<SelectListItem> ReasonCodeTypes { get; set; }
        public int ReasonCodeTypeId { get; set; }
        public ReasonCodeGroup ReasonCodeGroup { get; set; }

        public ProductReasonItemOrderTypeSelectionVM()
        {
        }
        public ProductReasonItemOrderTypeSelectionVM(IEnumerable<SelectListItem> reasonCodeTypes, int reasonCodeTypeId, ReasonCodeGroup reasonCodeGroup)
        {
            ReasonCodeTypeId = reasonCodeTypeId;
            ReasonCodeTypes = reasonCodeTypes;
            ReasonCodeGroup = reasonCodeGroup;
        }

    }
}

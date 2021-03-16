using CWTDesktopDatabase.Validation;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(FormOfPaymentAdviceMessageGroupItemValidation))]
    public partial class FormOfPaymentAdviceMessageGroupItem : CWTBaseModel
    {
		public string FormOfPaymentAdviceMessageGroupName { get; set; }
		public string ProductName { get; set; }
		public string SupplierName { get; set; }
		public string CountryName { get; set; }
		public string FormOfPaymentTypeDescription { get; set; }
        public string LanguageName { get; set; }
    }
}
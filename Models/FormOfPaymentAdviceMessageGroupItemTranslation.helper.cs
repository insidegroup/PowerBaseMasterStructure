using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(FormOfPaymentAdviceMessageGroupItemTranslationValidation))]
	public partial class FormOfPaymentAdviceMessageGroupItemTranslation : CWTBaseModel
    {
        public string LanguageName { get; set; }

		//No DB relationship
		public FormOfPaymentAdviceMessageGroupItem FormOfPaymentAdviceMessageGroupItem { get; set; }
    }
}

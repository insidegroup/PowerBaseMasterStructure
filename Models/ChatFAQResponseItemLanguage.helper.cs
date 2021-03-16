using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ChatFAQResponseItemLanguageValidation))]
	public partial class ChatFAQResponseItemLanguage : CWTBaseModel
    {
        public string LanguageName { get; set; }
    }
}

using CWTDesktopDatabase.Validation;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ChatFAQResponseItemValidation))]
    public partial class ChatFAQResponseItem : CWTBaseModel
    {
        public string ChatFAQResponseGroupName { get; set; }
        public string ChatMessageFAQName { get; set; }
        public string LanguageName { get; set; }
    }
}
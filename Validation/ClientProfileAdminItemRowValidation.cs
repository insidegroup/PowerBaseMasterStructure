using CWTDesktopDatabase.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ClientProfileAdminItemRowValidation
    {
		[RegularExpression(@"^([\w\s-.:\/()*\@\+\!\#\u2628\u00A4\‡\¥\,]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string DefaultGDSCommandFormat { get; set; }

    }
}
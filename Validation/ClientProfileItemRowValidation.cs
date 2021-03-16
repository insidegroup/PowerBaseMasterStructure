using CWTDesktopDatabase.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ClientProfileItemRowValidation
    {
        //[CWTRequiredIfNot("Remark", "", ErrorMessage = "Format Required")]
		[RegularExpression(@"^([\w\s-.:\/()*\u2628\u00A4\,]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string GDSCommandFormat { get; set; }

    }
}
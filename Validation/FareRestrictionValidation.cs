using System.ComponentModel.DataAnnotations;
using Foolproof;
using CWTDesktopDatabase.Helpers; 

namespace CWTDesktopDatabase.Validation
{
    public class FareRestrictionValidation
    {
        [Required(ErrorMessage = "Fare Basis Required")]
        [RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string FareBasis { get; set; }

        [RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string Changes { get; set; }

        [RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string Cancellations { get; set; }

        [RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string ReRoute { get; set; }

        [RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string ValidOn { get; set; }

        [RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string MinimumStay { get; set; }

        [RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string MaximumStay { get; set; }

        [Required(ErrorMessage = "Product Required")]
        public string ProductId { get; set; }

        [CWTRequiredIfNot("ProductId", "8", ErrorMessage = "Supplier is Required")]
        public string SupplierName { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }

    }
}

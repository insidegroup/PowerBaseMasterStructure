using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Foolproof;
using CWTDesktopDatabase.Helpers;
using DataAnnotationsExtensions;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Validation
{
	[Bind(Exclude = "CreationTimestamp")]
	public class CreditCardValidation
    {
        [Required(ErrorMessage = "Client Top Unit Required")]
        public string ClientTopUnitGuid { get; set; }

        [Required(ErrorMessage = "CreditCard Type Required")]
        public int CreditCardTypeId { get; set; }

        [CreditCard]
        [Required(ErrorMessage = "Credit Card Number Required")]
        [StringLength(16,MinimumLength=6, ErrorMessage="Must be at least 6 numbers")]
        public string CreditCardNumber { get; set; }

        [Required(ErrorMessage = "Valid To Date Required")]
        public DateTime? CreditCardValidTo { get; set; }

        [Required(ErrorMessage = "CreditCard Holder Required")]
        public string CreditCardHolderName { get; set; }

        [Required(ErrorMessage = "Vendor Required")]
        public string CreditCardVendorCode { get; set; }

        [Required(ErrorMessage = "Hierarchy Required")]
        public string HierarchyType { get; set; }

        [Required(ErrorMessage = "Hierarchy Item Required.")]
        public string HierarchyItem { get; set; }

        [RequiredIfRegExMatch("HierarchyType", "ClientSubUnitTravelerType", ErrorMessage = "TravelerType Required")]
        public string TravelerTypeGuid { get; set; }

		[RegularExpression(@"^[0-9]*$", ErrorMessage = "Only numerical values are allowed")]
		[StringLength(4, ErrorMessage = "Must be no longer than 4 numbers")]
		public string CVV { get; set; }

    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Models
{
    /*Copy of CreditCard but with no number*/
	public partial class CreditCardEditable : CWTBaseModel
    {
        public int CreditCardId { get; set; }
        public DateTime? CreditCardValidFrom { get; set; }
        public DateTime CreditCardValidTo { get; set; }
        public byte? CreditCardIssueNumber { get; set; }
        public string CreditCardHolderName { get; set; }
        public string CreditCardVendorCode { get; set; }
        public string ClientTopUnitGuid { get; set; }
        public int VersionNumber { get; set; }
        public int? CreditCardTypeId { get; set; }
        public string CreditCardToken { get; set; }
		public string MaskedCreditCardNumber { get; set; }

		public string CVV { get; set; }					//unencrypted version
		public string MaskedCVVNumber { get; set; }
		
		public int? ProductId { get; set; }
		public string ProductName { get; set; }
		
		public string SupplierCode { get; set; }
		public string SupplierName { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;


namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(CreditCardValidation))]
	public partial class CreditCard : CWTBaseModel
    {

        /*Depending on the Environment, there can be requirement for 1 of the following:
         * Valid CreditCardNumber Only Allowed
         * InValid CreditCardNumber Only Allowed
         * Either Valid or Invalid CreditCardNumber Allowed
         */
		public string CreditCardNumber { get; set; }	//unencrypted version
		public string CVV { get; set; }					//unencrypted version

        public string ClientTopUnitName { get; set; }
        public string CreditCardVendorName { get; set; }
        public string CreditCardTypeDescription { get; set; }
        public bool CanHaveRealCreditCardsFlag { get; set; }
        public bool WarningShownFlag { get; set; }
        
        //cannot get ViewModel working - adding these 5 instead - DMC May 2011
        public string ClientAccountName { get; set; }
        public string ClientAccountNumber { get; set; }
 
        public string HierarchyType { get; set; }    //Link to Hierarchy     eg Location or Country
        public string HierarchyItem { get; set; }   //Text Value            eg London or UK
        public string HierarchyCode { get; set; }   //Code                  eg LON or GB

        //only used when ClientSubUnitTravelerType is chosen
        public string TravelerTypeGuid { get; set; }
        public string ClientSubUnitGuid { get; set; }
        public string TravelerTypeName { get; set; }
        public string ClientSubUnitName { get; set; }

        //only used when a ClientAccount is chosen
        public string SourceSystemCode { get; set; }    //CLientAccountNumber is stored in HierarchyCode

		public string SupplierName { get; set; }
		public string ProductName { get; set; }

        public bool ValidateCreditCardNumber { get; set; } 
    }
    
    [MetadataType(typeof(CreditCardEditableValidation))]
    public partial class CreditCardEditable
    {

        /*Depending on the Environment, there can be requirement for 1 of the following:
         * Valid CreditCardNumber Only Allowed
         * InValid CreditCardNumber Only Allowed
         * Either Valid or Invalid CreditCardNumber Allowed
         */
        public string CreditCardNumber { get; set; }//unencrypted version

        public string ClientTopUnitName { get; set; }
        public string CreditCardVendorName { get; set; }
        public string CreditCardTypeDescription { get; set; }
        public bool CanHaveRealCreditCardsFlag { get; set; }
        public bool WarningShownFlag { get; set; }
        
        //cannot get ViewModel working - adding these 5 instead - DMC May 2011
        public string ClientAccountName { get; set; }
        public string ClientAccountNumber { get; set; }
 
        public string HierarchyType { get; set; }    //Link to Hierarchy     eg Location or Country
        public string HierarchyItem { get; set; }   //Text Value            eg London or UK
        public string HierarchyCode { get; set; }   //Code                  eg LON or GB

        //only used when ClientSubUnitTravelerType is chosen
        public string TravelerTypeGuid { get; set; }
        public string ClientSubUnitGuid { get; set; }
        public string TravelerTypeName { get; set; }
        public string ClientSubUnitName { get; set; }

        //only used when a ClientAccount is chosen
        public string SourceSystemCode { get; set; }    //CLientAccountNumber is stored in HierarchyCode

        public bool ValidateCreditCardNumber { get; set; } 
    }
}

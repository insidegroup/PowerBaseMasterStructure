using CWTDesktopDatabase.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.ViewModels
{

   
    public class ClientSubUnitCDRVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit{ get; set; }
        public ClientSubUnitClientDefinedReference ClientSubUnitClientDefinedReference{ get; set; }
        public string ClientDefinedReferenceItemId{ get; set; }
        public string DisplayName{ get; set; }
        
        public IEnumerable<SelectListItem> CreditCardSelectList { get; set; }
        public IEnumerable<SelectListItem> ClientAccountSelectList { get; set; }

        //public int CreditCardId { get; set; }
        public DateTime? CreditCardValidTo { get; set; }
        public string ClientAccountNumber { get; set; }


        public ClientSubUnitCDRVM()
        {
        }

        public ClientSubUnitCDRVM(
            
            string clientAccountNumber, 
            //int creditCardId, 
            IEnumerable<SelectListItem> creditCardSelectList, 
            IEnumerable<SelectListItem> clientAccountSelectList, 
            DateTime creditCardValidTo, 
            ClientSubUnitClientDefinedReference clientSubUnitClientDefinedReference, 
            ClientSubUnit clientSubUnit, 
            string clientDefinedReferenceItemId, string displayName)
        {
            ClientSubUnit = clientSubUnit;
            //CreditCardId = creditCardId;
            CreditCardValidTo = creditCardValidTo;
            ClientSubUnitClientDefinedReference = clientSubUnitClientDefinedReference;
            ClientDefinedReferenceItemId = clientDefinedReferenceItemId;
            CreditCardSelectList = creditCardSelectList;
            ClientAccountSelectList = clientAccountSelectList;
            ClientAccountNumber = clientAccountNumber;
            DisplayName = displayName;
        }
    }
}
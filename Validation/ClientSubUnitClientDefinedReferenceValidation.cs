using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ClientSubUnitClientDefinedReferenceValidation
    {
      [Required(ErrorMessage="Value Required")]
	  [RegularExpression(@"^([\w\s-()\*\.\&\@\/]+)$", ErrorMessage = "Special character entered is not allowed")]
      public int Value { get; set; }

      [AtLeastOneRequired("CreditCardId", "ClientAccountNumberSourceSystemCode",ErrorMessage = "Account and/or Credit Card Required")]
      public int CreditCardId { get; set; }
       
      [AtLeastOneRequired("ClientAccountNumberSourceSystemCode", "CreditCardId",ErrorMessage = "Account and/or Credit Card Required")]
      public int ClientAccountNumberSourceSystemCode { get; set; }
    }
}
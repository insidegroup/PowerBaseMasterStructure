using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Validation;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ClientSubUnitClientDefinedReferenceValidation))]
	public partial class ClientSubUnitClientDefinedReference : CWTBaseModel
    {
        public DateTime? CreditCardValidTo { get; set; }
        public string ClientAccountNumberSourceSystemCode { get; set; }
    }
}
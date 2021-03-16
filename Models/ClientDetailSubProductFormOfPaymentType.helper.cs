using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ClientDetailSubProductFormOfPaymentTypeValidation))]
	public partial class ClientDetailSubProductFormOfPaymentType : CWTBaseModel
    {
        public string ClientDetailName { get; set; }
        public string SubProductName { get; set; }
        public string FormOfPaymentTypeDescription { get; set; }
    }
}

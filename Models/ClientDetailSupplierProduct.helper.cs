using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ClientDetailSupplierProductValidation))]
	public partial class ClientDetailSupplierProduct : CWTBaseModel
    {
        public string ClientDetailName { get; set; }
        public string ProductName { get; set; }
        public string SupplierName { get; set; }

    }

     public class ClientDetailSupplierProductJSON
     {
         public string SupplierCode  { get; set; }
         public string ProductId { get; set; }
     }
}

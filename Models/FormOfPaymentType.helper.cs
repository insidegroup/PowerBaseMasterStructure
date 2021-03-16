using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    public class FormOfPaymentTypeJSON
    {
        public int FormOfPaymentTypeId { get; set; }
        public string FormOfPaymentTypeDescription { get; set; }
    }
}

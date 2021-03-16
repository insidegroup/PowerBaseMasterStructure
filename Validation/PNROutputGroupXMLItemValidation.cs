using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Validation
{
    public class PNROutputGroupXMLItemValidation
    {
        [Required(ErrorMessage = "Remark Type Required")]
        public string RemarkType { get; set; }

        [Required(ErrorMessage = "Bind Required")]
        public string Bind { get; set; }

        [Required(ErrorMessage = "Qualifier Required")]
        public string Qualifier { get; set; }

        [Required(ErrorMessage = "UpdateType Required")]
        public string UpdateType { get; set; }

        [Required(ErrorMessage = "GroupId Required")]
        public string GroupId { get; set; }

        [Required(ErrorMessage = "Value Required")]
        public string Value { get; set; }
    }
}

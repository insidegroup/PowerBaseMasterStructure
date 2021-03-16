using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PolicyRoutingValidation))]
	public partial class PolicyRouting : CWTBaseModel
    {
        public string FromCode { get; set; }
        public string FromCodeType { get; set; }
        public string FromName { get; set; }

        public string ToCode { get; set; }
        public string ToCodeType { get; set; }
        public string ToName { get; set; }


    }

    public partial class PolicyRoutingJSON
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public string CodeType { get; set; }
    }

}

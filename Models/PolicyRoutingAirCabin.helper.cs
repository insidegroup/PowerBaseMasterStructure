using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PolicyRoutingAirCabinValidation))]
	public partial class PolicyRoutingAirCabin : CWTBaseModel
    {
        public string FromCode { get; set; }
        public string FromCodeType { get; set; }

        public string ToCode { get; set; }
        public string ToCodeType { get; set; }


        //used to enforce validation when adding policyrouting to aircabingroupitem
        //string is easier to use than boolean, blank = no, "Required"=yes
        public string Mandatory { get; set; }
    }

    public partial class PolicyRoutingAirCabinJSON
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public string CodeType { get; set; }
    }

}

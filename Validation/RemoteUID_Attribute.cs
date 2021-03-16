using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
   // <snippet2>
    public sealed class RemoteUID_Attribute : ValidationAttribute {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string ParameterName { get; set; }
        public string RouteName { get; set; }

        public override bool IsValid(object value) {
            return true;
        }
    } 
   // </snippet2>


}

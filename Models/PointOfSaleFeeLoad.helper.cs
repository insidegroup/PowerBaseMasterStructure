using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PointOfSaleFeeLoadValidation))]
    public partial class PointOfSaleFeeLoad : CWTBaseModel
    {
        public string ClientTopUnitName { get; set; }
        public string ClientSubUnitName { get; set; }
        public string TravelerTypeName { get; set; }

        public Product Product { get; set; }
    }
}
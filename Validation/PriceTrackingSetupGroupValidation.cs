using CWTDesktopDatabase.Models;
using Foolproof;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class PriceTrackingSetupGroupValidation
    {
        [Required(ErrorMessage = "Hierarchy Required")]
        public string HierarchyType { get; set; }

        [Required(ErrorMessage = "Hierarchy Item Required.")]
        public string HierarchyItem { get; set; }

        [Required(ErrorMessage = "Setup Type Required")]
        public int PriceTrackingSetupTypeId { get; set; }

        [Required(ErrorMessage = "GDS Required")]
        public string GDSCode { get; set; }

        [Required(ErrorMessage = "PCC/OID Required")]
        public string PseudoCityOrOfficeId { get; set; }

        //Alphanumeric characters or one of the following 5 characters: ()*-_
        [RegularExpression(@"^([0-9A-z()*-_]+)$", ErrorMessage = "Special character entered is not allowed")]
        [Required(ErrorMessage = "FIQID Required")]
        public string FIQID { get; set; }
    }
}
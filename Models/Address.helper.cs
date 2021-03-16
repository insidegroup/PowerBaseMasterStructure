using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(AddressValidation))]
	public partial class Address : CWTBaseModel
    {
        public string CountryName { get; set; }
        public string StateProvinceCode { get; set; }
        public string MappingQualityDescription { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(OptionalFieldItemValidation))]
	public partial class OptionalFieldItem : CWTBaseModel
    {
		public string SupplierName { get; set; }
		public string ProductName { get; set; }
    }
}
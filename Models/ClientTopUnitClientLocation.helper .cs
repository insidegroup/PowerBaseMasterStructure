using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(ClientTopUnitClientLocationValidation))]
	public partial class ClientTopUnitClientLocation : CWTBaseModel
	{
	}
}
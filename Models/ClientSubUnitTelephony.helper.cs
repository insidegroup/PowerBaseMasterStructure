using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(ClientSubUnitTelephonyValidation))]
	public partial class ClientSubUnitTelephony : CWTBaseModel
	{
	}
}
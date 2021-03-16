using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ClientFeeItemValidation))]
	public partial class ClientFeeItem : CWTBaseModel
    {
    }
}
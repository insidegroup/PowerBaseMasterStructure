using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(QueueMinderItemValidation))]
	public partial class QueueMinderItem : CWTBaseModel
    {
    }
}

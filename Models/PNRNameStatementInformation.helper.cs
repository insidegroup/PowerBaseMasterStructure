using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(PNRNameStatementInformationValidation))]
	[Bind(Exclude = "CreationTimestamp")]
	public partial class PNRNameStatementInformation : CWTBaseModel
    {

	}
}
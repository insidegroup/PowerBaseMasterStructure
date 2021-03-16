using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(PolicyOtherGroupHeaderValidation))]
	public partial class PolicyOtherGroupHeader : CWTBaseModel
	{
		public string Label { get; set; }
		public string LabelLanguageCode { get; set; }

		public string TableName { get; set; }
		public string TableNameLanguageCode { get; set; }
	}
}
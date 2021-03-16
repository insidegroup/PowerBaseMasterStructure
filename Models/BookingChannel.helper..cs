using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(BookingChannelValidation))]
	public partial class BookingChannel : CWTBaseModel
	{
		public string UsageTypeDescription { get; set; }
	}
}
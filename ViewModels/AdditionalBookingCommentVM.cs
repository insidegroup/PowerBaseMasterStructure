using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "Languages")]
	public class AdditionalBookingCommentVM : CWTBaseViewModel
	{ 
        public ClientSubUnit ClientSubUnit { get; set; }
		public AdditionalBookingComment AdditionalBookingComment { get; set; }

		public IEnumerable<SelectListItem> Languages { get; set; }

        public AdditionalBookingCommentVM()
        {
        }

		public AdditionalBookingCommentVM(
			ClientSubUnit clientSubUnit,
			AdditionalBookingComment additionalBookingComment)
        {
            ClientSubUnit = clientSubUnit;
			AdditionalBookingComment = additionalBookingComment;
       }
	}
}
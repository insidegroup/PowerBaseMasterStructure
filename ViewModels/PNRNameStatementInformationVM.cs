using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PNRNameStatementInformationVM : CWTBaseViewModel
	{ 
        public ClientSubUnit ClientSubUnit { get; set; }
		public PNRNameStatementInformation PNRNameStatementInformation { get; set; }
        
		public IEnumerable<SelectListItem> GDSList { get; set; }
		public IEnumerable<SelectListItem> Delimiters { get; set; }
		public IEnumerable<SelectListItem> StatementInformationItems { get; set; }

		//Edit
		public IEnumerable<SelectListItem> Delimiter1 { get; set; }
		public IEnumerable<SelectListItem> StatementInformationItem1 { get; set; }
		public IEnumerable<SelectListItem> Delimiter2 { get; set; }
		public IEnumerable<SelectListItem> StatementInformationItem2 { get; set; }
		public IEnumerable<SelectListItem> Delimiter3 { get; set; }
		public IEnumerable<SelectListItem> StatementInformationItem3 { get; set; }
		public IEnumerable<SelectListItem> Delimiter4 { get; set; }
		public IEnumerable<SelectListItem> StatementInformationItem4 { get; set; }
		public IEnumerable<SelectListItem> Delimiter5 { get; set; }
		public IEnumerable<SelectListItem> StatementInformationItem5 { get; set; }
		public IEnumerable<SelectListItem> Delimiter6 { get; set; }

        public PNRNameStatementInformationVM()
        {
        }

		public PNRNameStatementInformationVM(
			PNRNameStatementInformation _PNRNameStatementInformation, 
			ClientSubUnit clientSubUnit)
        {
			PNRNameStatementInformation = _PNRNameStatementInformation;
			ClientSubUnit = clientSubUnit;
        }

	}
}
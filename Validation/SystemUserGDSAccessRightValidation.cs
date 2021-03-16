using System.ComponentModel.DataAnnotations;
using Foolproof;
using CWTDesktopDatabase.Helpers; 

namespace CWTDesktopDatabase.Validation
{
	public class SystemUserGDSAccessRightValidation
	{
		[Required(ErrorMessage = "GDS Required")]
		public string GDSCode { get; set; }

		//GDS Sign On ID is a free form text box that allows up to 10 characters including alphanumeric, all accented characters 
		//and allowed special characters asterisk (*), dash (-), underscore (_), space, period (.) and right and left parentheses (()).
		[Required(ErrorMessage = "GDS Sign On ID Required")]
		[RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string GDSSignOnID { get; set; }

		//TA/GTID/Certificate is a free form text box that allows up to 20 characters including alphanumeric, all accented characters 
		//and allowed special characters asterisk (*), dash (-), underscore (_), space, period (.) and right and left parenthesis (()).
		[RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string TAGTIDCertificate { get; set; }

		[Required(ErrorMessage = "GDS Access Type Required")]
		public string GDSAccessTypeId { get; set; }

		//Request ID is a free form text box that allows up to 20 characters including alphanumeric, all accented characters 
		//and allowed special characters asterisk (*), dash (-), underscore (_), space, period (.) and right and left parenthesis (()).
		[RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string RequestId { get; set; }

		//Internal Remarks is a free form text box that allows up to 1024 characters including alphanumeric, all accented characters 
		//and allowed special characters asterisk (*), dash (-), underscore (_), space, period (.) and right and left parenthesis (()).
		[RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string SystemUserGDSAccessRightInternalRemark { get; set; }
	}
}

using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class GDSOrderValidation
    {
		
		[Required(ErrorMessage = "GDS Required")]
		public string GDSCode { get; set; }

		[Required(ErrorMessage = "Order Date Required")]
		public string GDSOrderDateTime { get; set; }

		[Required(ErrorMessage = "Order Status Required")]
		public int GDSOrderStatusId { get; set; }
		
		[Required(ErrorMessage = "Order Type Required")]
		public int GDSOrderTypeId { get; set; }

		[Required(ErrorMessage = "PCC/OID Required")]
		public int PseudoCityOrOfficeMaintenanceId { get; set; }

		//Order Analyst Email allows alphanumeric, special characters and accented characters: slash, dash, period, underscore, apostrophe and the @ sign
		[Required(ErrorMessage = "Order Analyst Email Required")]
		[RegularExpression(@"^([À-ÿ\w\s\/\-_'.@]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string OrderAnalystEmail { get; set; }

		//Order Analyst Phone only allows numeric values (0 through 9, no dash, no plus, no leading or trailing spaces)
		[Required(ErrorMessage = "Order Analyst Phone Required")]
		[RegularExpression(@"^[0-9]+$", ErrorMessage = "Special character entered is not allowed")]
		public string OrderAnalystPhone { get; set; }

		[Required(ErrorMessage = "Order Analyst Country Required")]
		public string OrderAnalystCountryCode { get; set; }

		//Ticket Number allows alphanumeric, all accented characters and allowed special characters asterisk: (*), dash (-), underscore (_), space, period (.) and right and left parenthesis (())
		[Required(ErrorMessage = "Ticket Number Required")]
		[RegularExpression(@"^([À-ÿ\w\s*\-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string TicketNumber { get; set; }

		[Required(ErrorMessage = "CWT Cost Center Number Required")]
		public string CWTCostCenterNumber { get; set; }

		//Requester First Name allows special characters, accented characters and slash, dash, period and apostrophe
		[Required(ErrorMessage = "Requester First Name Required")]
		[RegularExpression(@"^([À-ÿ\w\s\/\-.']+)$", ErrorMessage = "Special character entered is not allowed")]
		public string RequesterFirstName { get; set; }

		//Requester Last Name allows special characters, accented characters and slash, dash, period and apostrophe
		[Required(ErrorMessage = "Requester Last Name Required")]
		[RegularExpression(@"^([À-ÿ\w\s\/\-.']+)$", ErrorMessage = "Special character entered is not allowed")]
		public string RequesterLastName { get; set; }

		//Requester Email allows alphanumeric, special characters and accented characters, can also allow slash, dash, period, underscore, apostrophe and the @ sign  
		[Required(ErrorMessage = "Requester Email Name Required")]
		[RegularExpression(@"^([À-ÿ\w\s\/\-_'.@]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string RequesterEmail { get; set; }

		//Requester Phone only allows numeric values (0 through 9, no dash, no plus, no leading or trailing spaces)
		[RegularExpression(@"^[0-9]+$", ErrorMessage = "Special character entered is not allowed")]
		[Required(ErrorMessage = "Requester Phone Required")]
		public string RequesterPhone { get; set; }

		[Required(ErrorMessage = "Requester Country Required")]
		public string RequesterCountryCode { get; set; }

		[Required(ErrorMessage = "Requester UID Required")]
		public string RequesterUID { get; set; }

		public string EmailToAddress { get; set; }

		//PseudoCityOrOfficeMaintenance validation only when GDSOrderTypeId is set to  New PCC / OID

		[CWTRequiredIf("ShowDataFlag", true, ErrorMessage = "IATA Required")]
		public int? PseudoCityOrOfficeMaintenance_IATAId { get; set; }

		[CWTRequiredIf("ShowDataFlag", true, ErrorMessage = "GDS Required")]
		public string PseudoCityOrOfficeMaintenance_GDSCode { get; set; }

		[CWTRequiredIf("ShowDataFlag", true, ErrorMessage = "PCC/OID Address Required")]
		public int? PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeAddressId { get; set; }

		[CWTRequiredIf("ShowDataFlag", true, ErrorMessage = "Location Contact Name Required")]
		[RegularExpression(@"^([\w\s*-_.()']+)$", ErrorMessage = "Special character entered is not allowed")]
		public string PseudoCityOrOfficeMaintenance_LocationContactName { get; set; }

		[CWTRequiredIf("ShowDataFlag", true, ErrorMessage = "Location Phone Required")]
		[RegularExpression(@"^([0-9]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string PseudoCityOrOfficeMaintenance_LocationPhone { get; set; }

		[CWTRequiredIf("ShowDataFlag", true, ErrorMessage = "Internal Site Name")]
		[RegularExpression(@"^([\w\s*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string PseudoCityOrOfficeMaintenance_InternalSiteName { get; set; }

		[CWTRequiredIf("ShowDataFlag", true, ErrorMessage = "External Name Required")]
		public int? PseudoCityOrOfficeMaintenance_ExternalNameId { get; set; }

		[CWTRequiredIf("ShowDataFlag", true, ErrorMessage = "Pseudo City/Office ID Type Required")]
		public int? PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeTypeId { get; set; }

		[CWTRequiredIf("ShowDataFlag", true, ErrorMessage = "Pseudo City/Office ID Location Type Required")]
		public int? PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeLocationTypeId { get; set; }
	}
}
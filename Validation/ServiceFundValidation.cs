using System;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class ServiceFundValidation
	{
		[Required(ErrorMessage = "Client TopUnit Name Required")]
		public string ClientTopUnitName { get; set; }

		[Required(ErrorMessage = "GDS Required")]
		public string GDSCode { get; set; }

		[Required(ErrorMessage = "Service Fund PCC/OID Required")]
		[RegularExpression(@"^([a-zA-Z0-9]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string ServiceFundPseudoCityOrOfficeId { get; set; }

		[Required(ErrorMessage = "PCC/OID Country Code Required")]
		public string PCCCountryCode { get; set; }

		[Required(ErrorMessage = "Fund Use Status Required")]
		public string FundUseStatus { get; set; }

		[Required(ErrorMessage = "Service Fund Queue Required")]
		[RegularExpression(@"^(\d{1,10})$", ErrorMessage = "Special character entered is not allowed")]
		public int ServiceFundQueue { get; set; }

		[Required(ErrorMessage = "Service Fund Start Time Required")]
		[RegularExpression(@"^(2[0-3]|[01]?[0-9]):([0-5]?[0-9]):?([0-5]?[0-9]?)$", ErrorMessage = "Format entered is not allowed")]
		public string ServiceFundStartTimeString { get; set; }

		[Required(ErrorMessage = "Service Fund End Time Required")]
		[RegularExpression(@"^(2[0-3]|[01]?[0-9]):([0-5]?[0-9]):?([0-5]?[0-9]?)$", ErrorMessage = "Format entered is not allowed")]
		public string ServiceFundEndTimeString { get; set; }

		[Required(ErrorMessage = "Time Zone Required")]
		public string TimeZoneRuleCode { get; set; }

		[Required(ErrorMessage = "Client Account Number Required")]
		public string ClientAccountNumber { get; set; }

		[Required(ErrorMessage = "Product Required")]
		public int ProductId { get; set; }

		[Required(ErrorMessage = "Supplier Required")]
		public int SupplierCode { get; set; }

		[RegularExpression(@"^([a-zA-Z0-9]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string ServiceFundSavingsType { get; set; }

		[Required(ErrorMessage = "Service Fund Minimum Value Required")]
		[RegularExpression(@"^(\d{1,8}|\d{0,8}\.\d{1,4})$", ErrorMessage = "This field allows up to 8 digits and 4 decimal places")]
		public decimal? ServiceFundMinimumValue { get; set; }

		[Required(ErrorMessage = "Service Fund Currency Required")]
		public string ServiceFundCurrencyCode { get; set; }

		[Required(ErrorMessage = "Service Fund Routing Required")]
		public string ServiceFundRouting { get; set; }

		[RegularExpression(@"^([a-zA-Z0-9]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string ServiceFundClass { get; set; }

		[Required(ErrorMessage = "Channel Type Required")]
		public int ServiceFundChannelTypeId { get; set; }
	}
}
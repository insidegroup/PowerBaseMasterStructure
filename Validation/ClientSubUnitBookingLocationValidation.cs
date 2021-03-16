using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
    public class BookingChannelValidation
    {
		[Required(ErrorMessage = "Booking Channel Required")]
		public int BookingChannelTypeId { get; set; }

		[Required(ErrorMessage = "Product Channel Required")]
		public int ProductChannelTypeId { get; set; }

		[Required(ErrorMessage = "GDS Required")]
		public int GDSCode { get; set; }

		[RegularExpression(@"^[0-9a-zA-Z]+$", ErrorMessage = "Special character entered is not allowed")]
		public string BookingPseudoCityOrOfficeId { get; set; }

		[RegularExpression(@"^[0-9a-zA-Z]+$", ErrorMessage = "Special character entered is not allowed")]
		public string TicketingPseudoCityOrOfficeId { get; set; }

    }
}
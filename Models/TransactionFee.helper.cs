using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;


namespace CWTDesktopDatabase.Models
{
  //[MetadataType(typeof(TransactionFeeValidation))]
	public partial class TransactionFee : CWTBaseModel
    {
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
        public string TravelIndicatorDescription { get; set; }
        public string TravelerBackOfficeTypeDescription { get; set; }
        public string TripTypeClassificationDescription { get; set; }
        public string ChargeTypeDescription { get; set; }
        public string BookingOriginationDescription { get; set; }
        public string BookingSourceDescription { get; set; }
        public string PolicyLocationName { get; set; }
        public string FeeCurrencyName { get; set; }
        public string TicketPriceCurrencyName { get; set; }
        public bool IncursGSTFlagNonNullable { get; set; } //IncursGSTFlag is nullable
    }

    [MetadataType(typeof(TransactionFeeAirValidation))]
    public class TransactionFeeAir : TransactionFee
    {
    }

    [MetadataType(typeof(TransactionFeeCarHotelValidation))]
    public class TransactionFeeCarHotel : TransactionFee
    {
    }

	public class TransactionFeeJson {
		public int TransactionFeeId;
		public string TransactionFeeDescription;
	}
}
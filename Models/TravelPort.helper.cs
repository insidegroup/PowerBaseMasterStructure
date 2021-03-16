using CWTDesktopDatabase.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(TravelPortValidation))]
	public partial class TravelPort : CWTBaseModel
    {
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string TravelPortTypeDescription { get; set; }
		public decimal LatitudeDecimal { get; set; }
		public decimal LongitudeDecimal { get; set; }
	}
    
	public class TravelPortJSON
    {
        public string TravelPortName { get; set; }
        public string TravelPortCode { get; set; }
    }

	public partial class TravelPortReference
	{
		public string TableName { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(CityValidation))]
	public partial class City : CWTBaseModel
    {
        public string CountryName { get; set; }

		public decimal LatitudeDecimal { get; set; }
		public decimal LongitudeDecimal { get; set; }

        public TimeZoneRule TimeZoneRule { get; set; }
    }

	public partial class CityReference
	{
		public string TableName { get; set; }
	}

}

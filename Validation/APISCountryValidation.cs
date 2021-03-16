using System.ComponentModel.DataAnnotations;
using System;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Validation
{
	[Bind(Include = "OriginCountryCode, DestinationCountryCode, StartDate")]
	public class APISCountryValidation
    {
        [Required(ErrorMessage = "Origin Country Required")]
        public string OriginCountryCode { get; set; }

        [Required(ErrorMessage = "Destination Country Required")]
        public string DestinationCountryCode { get; set; }

        [Required(ErrorMessage = "StartDate Required")]
        public DateTime? StartDate { get; set; }

    }
}

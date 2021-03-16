using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PolicyLocationValidation))]
	public partial class PolicyLocation : CWTBaseModel
    {
        public string TravelPortName { get; set; }
        public string TravelPortType { get; set; }
        public string LocationType { get; set; }          //city or country or globalsubregion etc
        public string LocationName { get; set; }            //"London" or "Ireland" or "Europe" etc
        public string LocationCode { get; set; }            //"LON" or "IE" or "EUR" etc

		public string PolicyLocationValue { get; set; }
		public string ParentName { get; set; }
    }
    public partial class PolicyLocationLocationJSON
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public string CodeType { get; set; }
    }
}

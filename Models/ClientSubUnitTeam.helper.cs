using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ClientSubUnitTeamValidation))]
	public partial class ClientSubUnitTeam : CWTBaseModel
    {
        public string TeamName { get; set; }
        public string ClientSubUnitName { get; set; }
		public bool IsPrimaryTeamForSub { get; set; }
    }
}

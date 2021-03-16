using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;
namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(AdministratorRoleHierarchyLevelTypeValidation))]
	public partial class AdministratorRoleHierarchyLevelType : CWTBaseModel
    {
        public string SystemUserGuid { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;


namespace CWTDesktopDatabase.Models
{
	public partial class WorkFlowItem : CWTBaseModel
    {
        public string WorkFlowGroupName { get; set; }
        public string FormName { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ClientDetailESCInformationValidation))]
	public partial class ClientDetailESCInformation : CWTBaseModel
    {
    }
}
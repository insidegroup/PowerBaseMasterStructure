using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(TicketQueueItemValidation))]
	public partial class TicketQueueItem : CWTBaseModel
    {
        public string TicketQueueGroupName { get; set; }
        public string TicketTypeDescription { get; set; }
        public string GDSName { get; set; }
    }
}

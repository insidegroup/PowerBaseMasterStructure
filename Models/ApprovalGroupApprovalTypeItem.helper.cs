using System.Collections.Generic;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    public partial class ApprovalGroupApprovalTypeItem : CWTBaseModel
    {
        public IEnumerable<SelectListItem> ApprovalGroupApprovalTypes { get; set; }

    }
}

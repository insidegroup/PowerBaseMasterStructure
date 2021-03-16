using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientProfileItemVM : CWTBaseViewModel
    {
        public IEnumerable<SelectListItem> ClientProfileMoveStatuses { get; set; }
        public ClientProfileItemRow ClientProfileItem { get; set; }
        

        public ClientProfileItemVM()
        {
        }
        public ClientProfileItemVM(ClientProfileItemRow clientProfileItem, IEnumerable<SelectListItem> clientProfileMoveStatuses)
        {
            ClientProfileMoveStatuses = clientProfileMoveStatuses;
            ClientProfileItem = clientProfileItem;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientProfileAdminItemVM : CWTBaseViewModel
    {
        public IEnumerable<SelectListItem> ClientProfileMoveStatuses { get; set; }
        public ClientProfileAdminItemRow ClientProfileAdminItem { get; set; }
        

        public ClientProfileAdminItemVM()
        {
        }
        public ClientProfileAdminItemVM(ClientProfileAdminItemRow clientProfileAdminItem, IEnumerable<SelectListItem> clientProfileMoveStatuses)
        {
            ClientProfileMoveStatuses = clientProfileMoveStatuses;
            ClientProfileAdminItem = clientProfileAdminItem;
        }
    }
}
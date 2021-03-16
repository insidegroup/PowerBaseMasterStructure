using System;
using System.Collections.Generic;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientProfileDataElementAdminItemVM : CWTBaseViewModel
    {
        public ClientProfileDataElement ClientProfileDataElement { get; set; }
        public ClientProfileAdminItem ClientProfileAdminItem { get; set; }

        public ClientProfileDataElementAdminItemVM()
        {
        }
        public ClientProfileDataElementAdminItemVM(
                                ClientProfileDataElement clientProfileDataElement,
                                ClientProfileAdminItem clientProfileAdminItem
                                )
        {
            ClientProfileDataElement = clientProfileDataElement;
            ClientProfileAdminItem = clientProfileAdminItem;
        }
       
    }
}
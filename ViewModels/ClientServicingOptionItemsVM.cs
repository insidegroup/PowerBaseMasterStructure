using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientServicingOptionItemsVM : CWTBaseViewModel
    {
        public List<ServicingOptionVM> ServicingOptionVMs { get; set; }
        public IEnumerable<SelectListItem> GDSs { get; set; }
        public bool ServicingOptionWriteAccess { get; set; }
        public int ServicingOptionGroupId { get; set; }
        
        public ClientServicingOptionItemsVM()
        {
            this.ServicingOptionVMs = new List<ServicingOptionVM>();
        }
        public ClientServicingOptionItemsVM(int servicingOptionGroupId, List<ServicingOptionVM> servicingOptionVMs, IEnumerable<SelectListItem> gDSs, bool servicingOptionWriteAccess)
        {
            ServicingOptionGroupId = servicingOptionGroupId;
            ServicingOptionVMs = servicingOptionVMs;
            GDSs = gDSs;
            ServicingOptionWriteAccess = servicingOptionWriteAccess;
        }

        public void Add(ServicingOptionVM servicingOptionVM)
        {
            ServicingOptionVMs.Add(servicingOptionVM);
        }
    }

    public class ServicingOptionVM : CWTBaseViewModel
    {
        public ServicingOptionItem ServicingOptionItem { get; set; }
		public bool ShowEditParameterButton { get; set; }

        //contains values from ServicingOptionItemValue table if exists, otherwise empty
        public List<ServicingOptionItemValue> ServicingOptionItemValues { get; set; }
    }
}

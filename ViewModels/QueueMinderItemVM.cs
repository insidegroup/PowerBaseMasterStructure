using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class QueueMinderItemVM : CWTBaseViewModel
   {
       public QueueMinderItem QueueMinderItem { get; set; }
       public IEnumerable<SelectListItem> GDSs { get; set; }
        public IEnumerable<SelectListItem> Contexts { get; set; }
        public IEnumerable<SelectListItem> QueueMinderTypes { get; set; }


        public QueueMinderItemVM()
        {
        }
        public QueueMinderItemVM(
                        QueueMinderItem queueMinderItem,
                        IEnumerable<SelectListItem> gDSs,
                        IEnumerable<SelectListItem> contexts,
                        IEnumerable<SelectListItem> queueMinderTypes
                        )
        {
            QueueMinderItem = queueMinderItem;
            GDSs = gDSs;
            Contexts = contexts;
            QueueMinderTypes = queueMinderTypes;
        }
       
    }
}
using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class QueueMinderGroupVM : CWTBaseViewModel
   {
        public QueueMinderGroup QueueMinderGroup { get; set; }
        public IEnumerable<SelectListItem> TripTypes { get; set; }
        public IEnumerable<SelectListItem> HierarchyTypes { get; set; }
        

        public QueueMinderGroupVM()
        {
        }
        public QueueMinderGroupVM(
                        QueueMinderGroup queueMinderGroup,
                        IEnumerable<SelectListItem> tripTypes,
                        IEnumerable<SelectListItem> hierarchyTypes
                        )
        {
            QueueMinderGroup = queueMinderGroup;
            TripTypes = tripTypes;
            HierarchyTypes = hierarchyTypes;
        }
       
    }
}
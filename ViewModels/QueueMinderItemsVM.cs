using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class QueueMinderItemsVM : CWTBaseViewModel
    {
        public QueueMinderGroup QueueMinderGroup { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectQueueMinderItems_v1Result> QueueMinderItems { get; set; }
        
        public QueueMinderItemsVM()
        {
        }
        public QueueMinderItemsVM(QueueMinderGroup queueMinderGroup, CWTPaginatedList<spDesktopDataAdmin_SelectQueueMinderItems_v1Result> queueMinderItems)
        {
            QueueMinderGroup = queueMinderGroup;
            QueueMinderItems = queueMinderItems;
        }
    }
}
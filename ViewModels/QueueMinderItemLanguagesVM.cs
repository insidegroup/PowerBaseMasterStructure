using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class QueueMinderItemLanguagesVM : CWTBaseViewModel
    {
        public QueueMinderItem QueueMinderItem { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectQueueMinderItemLanguage_v1Result> QueueMinderItemLanguages { get; set; }
        public bool HasWriteAccess { get; set; }

        public QueueMinderItemLanguagesVM()
        {
            HasWriteAccess = false;
        }
        public QueueMinderItemLanguagesVM(bool hasWriteAccess, QueueMinderItem queueMinderItem, CWTPaginatedList<spDesktopDataAdmin_SelectQueueMinderItemLanguage_v1Result> queueMinderItemLanguages)
        {
            QueueMinderItem = queueMinderItem;
            QueueMinderItemLanguages = queueMinderItemLanguages;
            HasWriteAccess = hasWriteAccess;
        }
    }
}
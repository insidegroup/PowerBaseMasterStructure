using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
    public class QueueMinderItemLanguageVM : CWTBaseViewModel
    {
        public QueueMinderItemLanguage QueueMinderItemLanguage { get; set; }
        public IEnumerable<SelectListItem> QueueMinderItemLanguages { get; set; }
        
        public QueueMinderItemLanguageVM()
        {
        }
        public QueueMinderItemLanguageVM(QueueMinderItemLanguage queueMinderItemLanguage, IEnumerable<SelectListItem> queueMinderItemLanguages)
        {
            QueueMinderItemLanguage = queueMinderItemLanguage;
            QueueMinderItemLanguages = queueMinderItemLanguages;
        }
    }
}
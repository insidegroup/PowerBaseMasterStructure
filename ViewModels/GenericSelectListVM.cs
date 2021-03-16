using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
    /*
     *A Class containing a Select List and a String
     * 
     * 
     */
    public class GenericSelectListVM : CWTBaseViewModel
    {
        public IEnumerable<SelectListItem> SelectList { get; set; }
        public string Message { get; set; }
        
        public GenericSelectListVM()
        {
        }
        public GenericSelectListVM(IEnumerable<SelectListItem> selectList, string message)
        {
            SelectList = selectList;
            Message = Message;
        }
    }
}

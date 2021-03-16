using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    
    public class XMLVM : CWTBaseViewModel
    {
        public XmlDocument XML { get; set; }
        
        public XMLVM()
        {
        }
        public XMLVM(XmlDocument xml)
        {
            XML = xml;
        }
    }
}

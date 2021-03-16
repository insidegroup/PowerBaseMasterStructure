using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Models
{
    /*NOT IN USE - to replace string URL in Delete Error Trapping
     * Need:
     * MessageForUser 
     * Route Collection
     * Route Action
     * Route Values
     * NavMenu/Section to keep open 
     * 
     * 
     * 
     */
    public class VersioningError
    {
        public string MessageForUser { get; set; }
        public string RedirectRoute { get; set; }
        public string RedirectController { get; set; }
        Dictionary<string, string> RouteValues = new Dictionary<string, string>();
        
    }
}

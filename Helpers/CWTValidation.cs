using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Helpers
{
    public class CWTValidation
    {
        /*
        only used in ClientSummaryRepository - checked if needed
         * */
        //convert SQL wildcards to literals
        public string SanitiseSQL( string sqlString)
        {
            string sanitisedSQL = sqlString.Replace("[", "[[]");
            sanitisedSQL = sanitisedSQL.Replace("_", "[_]");
            sanitisedSQL = sanitisedSQL.Replace("%", "[%]");
            //sanitisedSQL = sanitisedSQL.Replace("'", "''");
            return sanitisedSQL;
        }
    }
}

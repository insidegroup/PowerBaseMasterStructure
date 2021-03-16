using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Helpers
{
    public static class Tracking
    {
        private static string GetUrlParameter(string request, string parName)
        {
            string result = string.Empty;

            var urlParameters = HttpUtility.ParseQueryString(request);
            if (urlParameters.AllKeys.Contains(parName))
            {
                result = urlParameters.Get(parName);
            }

            return result;
        }

        public static string GetCurrentClientSubUnitGuid()
        {
            string clientSubUnitGuid = string.Empty;

            string id = GetUrlParameter(HttpContext.Current.Request.Url.Query.ToString(), "id");
            if (!string.IsNullOrEmpty(id))
            {
                ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
                ClientSubUnit clientSubUnit = clientSubUnitRepository.GetClientSubUnit(id);
                if (clientSubUnit != null)
                {
                    clientSubUnitGuid = clientSubUnit.ClientSubUnitGuid;
                }
            }

            return clientSubUnitGuid;
        }

        public static string GetCurrentClientTopUnitGuid()
        {
            string clientTopUnitGuid = string.Empty;

            string id = GetUrlParameter(HttpContext.Current.Request.Url.Query.ToString(), "id");
            if (!string.IsNullOrEmpty(id))
            {
                ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
                ClientTopUnit clientTopUnit = clientTopUnitRepository.GetClientTopUnit(id);
                if (clientTopUnit != null)
                {
                    clientTopUnitGuid = clientTopUnit.ClientTopUnitGuid;
                }
            }

            return clientTopUnitGuid;
        }
    }
}
using CWTDesktopDatabase.Models;
using System.Web.Mvc;
using System.Collections.Generic;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitTelephonyVM : CWTBaseViewModel
    {
        public ClientSubUnitTelephony ClientSubUnitTelephony { get; set; }
        public ClientSubUnit ClientSubUnit { get; set; }
        public IEnumerable<SelectListItem> CallerEnteredDigitDefinitionTypes { get; set; }
        
        public ClientSubUnitTelephonyVM()
        {
        }
        public ClientSubUnitTelephonyVM(ClientSubUnitTelephony clientSubUnitTelephony, ClientSubUnit clientSubUnit, IEnumerable<SelectListItem> callerEnteredDigitDefinitionTypes)
        {
            ClientSubUnitTelephony = clientSubUnitTelephony;
            ClientSubUnit = clientSubUnit;
            CallerEnteredDigitDefinitionTypes = callerEnteredDigitDefinitionTypes;
        }
    }
}
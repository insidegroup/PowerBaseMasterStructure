using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientSubUnitReasonCodesVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public bool Inherited { get; set; }
        public bool PolicyGroupWriteAccess { get; set; }
        public bool ReasonCodeGroupWriteAccess { get; set; }

		public List<ClientSubUnitReasonCodeProductGroup> ClientSubUnitReasonCodeProductGroup { get; set; }
		
		public ClientSubUnitReasonCodesVM()
        {
		}
        
		public ClientSubUnitReasonCodesVM(ClientSubUnit clientSubUnit, bool inherited, List<spDDAWizard_SelectClientSubUnitReasonCodes_v1Result> clientSubUnitReasonCodes, bool policyGroupWriteAccess, bool reasonCodeGroupWriteAccess)
        {
            ClientSubUnit = clientSubUnit;
            Inherited = inherited;
            PolicyGroupWriteAccess = policyGroupWriteAccess;
            ReasonCodeGroupWriteAccess = reasonCodeGroupWriteAccess;
        } 
	}

	public class ClientSubUnitReasonCodeProductGroup
	{
		public List<spDDAWizard_SelectClientSubUnitReasonCodesByProductAndType_v1Result> ReasonCodeProductGroups { get; set; }
		public Product Product { get; set; }
		public ReasonCodeType ReasonCodeType { get; set; }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.ViewModels
{
	[Bind(Exclude = "HasWriteAccess, HasCDRLinkImportAccess")]
	public class ClientSubUnitCDRsVM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitCDR_v1Result> ClientDefinedReferences { get; set; }
		//public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitCDRValidate_v1Result> ValidateClientDefinedReferences { get; set; }

        public IEnumerable<SelectListItem> ClientDefinedReferenceItems { get; set; }

        [Required(ErrorMessage = "Required")]
        public string ClientDefinedReferenceItemId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string DisplayName { get; set; }
        public string ClientSubUnitGuid { get; set; }
        public bool HasWriteAccess { get; set; }
        public bool HasCDRLinkImportAccess { get; set; }

		//ClientSubUnitClientDefinedReferenceItems
		public string RelatedToDisplayName { get; set; }
		public int ClientSubUnitClientDefinedReferenceItemId { get; set; }
		public List<ClientSubUnitClientDefinedReferenceItem> ClientSubUnitClientDefinedReferenceItems { get; set; }

        public ClientSubUnitCDRsVM()
        {
        }
        public ClientSubUnitCDRsVM(ClientSubUnit clientSubUnit, string clientDefinedReferenceItemId, CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitCDR_v1Result> clientDefinedReferences,
            IEnumerable<SelectListItem> clientDefinedReferenceItems, string displayName, bool hasWriteAccess, bool hasCDRLinkImportAccess)
        {
            ClientSubUnit = clientSubUnit;
            ClientDefinedReferenceItems = clientDefinedReferenceItems;
            ClientDefinedReferenceItemId = clientDefinedReferenceItemId;
            ClientDefinedReferences = clientDefinedReferences;
            DisplayName = displayName;
            HasWriteAccess = hasWriteAccess;
            HasCDRLinkImportAccess = hasCDRLinkImportAccess;
        }
    }
}
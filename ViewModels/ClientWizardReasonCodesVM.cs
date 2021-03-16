using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientWizardReasonCodesVM : CWTBaseViewModel
    {
        public List<spDDAWizard_SelectReasonCodeGroupReasonCodeItems_v1Result> ReasonCodes { get; set; }
        public List<spDDAWizard_SelectReasonCodeGroupAvailableReasonCodes_v1Result> AvailableReasonCodes { get; set; }
        public int ReasonCodeGroupId { get; set; }
        public int ProductId { get; set; }
        public int ReasonCodeTypeId { get; set; }

        public ClientWizardReasonCodesVM()
        {
        }

		public ClientWizardReasonCodesVM(
            List<spDDAWizard_SelectReasonCodeGroupReasonCodeItems_v1Result> reasonCodes, 
            List<spDDAWizard_SelectReasonCodeGroupAvailableReasonCodes_v1Result> availableReasonCodes,
            int reasonCodeGroupId,
            int productId,
            int reasonCodeTypeId
            )
        {
            ReasonCodes = reasonCodes;
            AvailableReasonCodes = availableReasonCodes;
            ReasonCodeGroupId = reasonCodeGroupId;
            ProductId = productId;
            ReasonCodeTypeId = reasonCodeTypeId;
        }
    }
}
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientFeeGroupsVM : CWTBaseViewModel
   {
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroups_v1Result> ClientFeeGroups { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroupsOrphaned_v1Result> ClientFeeGroupsOrphaned { get; set; }
        public string FeeTypeDisplayName { get; set; }
        public string FeeTypeDisplayNameShort { get; set; }
        public int FeeTypeId { get; set; }
        public bool HasDomainWriteAccess { get; set; }
 
        public ClientFeeGroupsVM()
        {
            HasDomainWriteAccess = false;
        }
        public ClientFeeGroupsVM(CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroups_v1Result> clientFeeGroups, 
            CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroupsOrphaned_v1Result> clientFeeGroupsOrphaned, 
            int feeTypeId, string feeTypeDisplayName, string feeTypeDisplayNameShort, bool hasDomainWriteAccess)
        {
            ClientFeeGroups = clientFeeGroups;
            ClientFeeGroupsOrphaned = clientFeeGroupsOrphaned;
            FeeTypeId = feeTypeId;
            FeeTypeDisplayName = feeTypeDisplayName;
            FeeTypeDisplayNameShort = feeTypeDisplayNameShort;
            HasDomainWriteAccess = hasDomainWriteAccess;
        }
    }
}
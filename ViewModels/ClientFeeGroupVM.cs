using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientFeeGroupVM : CWTBaseViewModel
    {
        public ClientFeeGroup ClientFeeGroup { get; set; }
        public IEnumerable<SelectListItem> TripTypes { get; set; }
        public IEnumerable<SelectListItem> HierarchyTypes { get; set; }
        public string FeeTypeDisplayName { get; set; }
        public string FeeTypeDisplayNameShort { get; set; }
        public int FeeTypeId { get; set; }

        public ClientFeeGroupVM()
        {
        }
        public ClientFeeGroupVM(ClientFeeGroup clientFeeGroup, IEnumerable<SelectListItem> tripTypes,
                        IEnumerable<SelectListItem> hierarchyTypes, int feeTypeId, string feeTypeDisplayName, string feeTypeDisplayNameShort)
        {
            ClientFeeGroup = clientFeeGroup;
            TripTypes = tripTypes;
            HierarchyTypes = hierarchyTypes;
            FeeTypeId = feeTypeId;
            FeeTypeDisplayName = feeTypeDisplayName;
            FeeTypeDisplayNameShort = feeTypeDisplayNameShort;
        }
    }
}
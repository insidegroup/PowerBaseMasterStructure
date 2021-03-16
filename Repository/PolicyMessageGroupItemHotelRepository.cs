using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyMessageGroupItemHotelRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Add
        public void Add(PolicyMessageGroupItemHotel policyMessageGroupItemHotel)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyMessageGroupItemHotel_v1(
                policyMessageGroupItemHotel.PolicyGroupId,
                policyMessageGroupItemHotel.PolicyMessageGroupItemName,
                policyMessageGroupItemHotel.SupplierCode,
                policyMessageGroupItemHotel.EnabledFlag,
                policyMessageGroupItemHotel.EnabledDate,
                policyMessageGroupItemHotel.ExpiryDate,
                policyMessageGroupItemHotel.TravelDateValidFrom,
                policyMessageGroupItemHotel.TravelDateValidTo,
                policyMessageGroupItemHotel.PolicyLocationId,
                adminUserGuid
            );
        }

        public void Edit(PolicyMessageGroupItemHotel policyMessageGroupItemHotel)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePolicyMessageGroupItemHotel_v1(
                policyMessageGroupItemHotel.PolicyMessageGroupItemId,
                policyMessageGroupItemHotel.PolicyMessageGroupItemName,
                policyMessageGroupItemHotel.SupplierCode,
                policyMessageGroupItemHotel.EnabledFlag,
                policyMessageGroupItemHotel.EnabledDate,
                policyMessageGroupItemHotel.ExpiryDate,
                policyMessageGroupItemHotel.TravelDateValidFrom,
                policyMessageGroupItemHotel.TravelDateValidTo,
                policyMessageGroupItemHotel.PolicyLocationId,
                adminUserGuid,
                policyMessageGroupItemHotel.VersionNumber
            );
        }

        public void Delete(PolicyMessageGroupItemHotel policyMessageGroupItemHotel)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyMessageGroupItemHotel_v1(
                policyMessageGroupItemHotel.PolicyMessageGroupItemId,
                adminUserGuid,
                policyMessageGroupItemHotel.VersionNumber
            );
        }

        //Get one PolicyGroup
        public PolicyMessageGroupItemHotel GetPolicyMessageGroupItemHotel(int policyMessageGroupItemId)
        {

            return (from n in db.spDesktopDataAdmin_SelectPolicyMessageGroupItemHotel_v1(policyMessageGroupItemId)
                    select new
                        PolicyMessageGroupItemHotel
                    {
                        PolicyMessageGroupItemId = n.PolicyMessageGroupItemId,
                        PolicyGroupId = n.PolicyGroupId,
                        SupplierCode = n.SupplierCode,
                        SupplierName = n.SupplierName,
                        PolicyLocationName = n.PolicyLocationName,
                        ProductId = n.ProductId,
                        EnabledFlag = n.EnabledFlag,
                        EnabledDate = n.EnabledDate,
                        ExpiryDate = n.ExpiryDate,
                        TravelDateValidFrom = n.TravelDateValidFrom,
                        TravelDateValidTo = n.TravelDateValidTo,
                        PolicyLocationId = n.PolicyLocationId,
                        VersionNumber = n.VersionNumber,
                        PolicyMessageGroupItemName = n.PolicyMessageGroupItemName
                    }).FirstOrDefault();
        }
    }
}
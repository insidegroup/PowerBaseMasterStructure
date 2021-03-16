using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyMessageGroupItemAirRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Add
        public void Add(PolicyMessageGroupItemAir policyMessageGroupItemAir, PolicyRouting policyRouting)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyMessageGroupItemAir_v1(
                policyMessageGroupItemAir.PolicyGroupId,
                policyMessageGroupItemAir.PolicyMessageGroupItemName,
                policyMessageGroupItemAir.SupplierCode,
                policyMessageGroupItemAir.EnabledFlag,
                policyMessageGroupItemAir.EnabledDate,
                policyMessageGroupItemAir.ExpiryDate,
                policyMessageGroupItemAir.TravelDateValidFrom,
                policyMessageGroupItemAir.TravelDateValidTo,
                policyRouting.Name,
                policyRouting.FromGlobalFlag,
                policyRouting.FromGlobalRegionCode,
                policyRouting.FromGlobalSubRegionCode,
                policyRouting.FromCountryCode,
                policyRouting.FromCityCode,
                policyRouting.FromTravelPortCode,
                policyRouting.FromTraverlPortTypeId,
                policyRouting.ToGlobalFlag,
                policyRouting.ToGlobalRegionCode,
                policyRouting.ToGlobalSubRegionCode,
                policyRouting.ToCountryCode,
                policyRouting.ToCityCode,
                policyRouting.ToTravelPortCode,
                policyRouting.ToTravelPortTypeId,
                policyRouting.RoutingViceVersaFlag,
                adminUserGuid
            );
        }

        public void Edit(PolicyMessageGroupItemAir policyMessageGroupItemAir, PolicyRouting policyRouting)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePolicyMessageGroupItemAir_v1(
                policyMessageGroupItemAir.PolicyMessageGroupItemId,
                policyMessageGroupItemAir.PolicyMessageGroupItemName,
                policyMessageGroupItemAir.SupplierCode,
                policyMessageGroupItemAir.EnabledFlag,
                policyMessageGroupItemAir.EnabledDate,
                policyMessageGroupItemAir.ExpiryDate,
                policyMessageGroupItemAir.TravelDateValidFrom,
                policyMessageGroupItemAir.TravelDateValidTo,
                policyRouting.Name,
                policyRouting.FromGlobalFlag,
                policyRouting.FromGlobalRegionCode,
                policyRouting.FromGlobalSubRegionCode,
                policyRouting.FromCountryCode,
                policyRouting.FromCityCode,
                policyRouting.FromTravelPortCode,
                policyRouting.FromTraverlPortTypeId,
                policyRouting.ToGlobalFlag,
                policyRouting.ToGlobalRegionCode,
                policyRouting.ToGlobalSubRegionCode,
                policyRouting.ToCountryCode,
                policyRouting.ToCityCode,
                policyRouting.ToTravelPortCode,
                policyRouting.ToTravelPortTypeId,
                policyRouting.RoutingViceVersaFlag,
                adminUserGuid,
                policyMessageGroupItemAir.VersionNumber
            );
        }

        public void Delete(PolicyMessageGroupItemAir policyMessageGroupItemAir)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyMessageGroupItemAir_v1(
                policyMessageGroupItemAir.PolicyMessageGroupItemId,
                adminUserGuid,
                policyMessageGroupItemAir.VersionNumber
            );
        }

        //Get one PolicyGroup
        public PolicyMessageGroupItemAir GetPolicyMessageGroupItemAir(int policyMessageGroupItemId)
        {

            return (from n in db.spDesktopDataAdmin_SelectPolicyMessageGroupItemAir_v1(policyMessageGroupItemId)
                    select new
                        PolicyMessageGroupItemAir
                    {
                        PolicyMessageGroupItemId = n.PolicyMessageGroupItemId,
                        PolicyGroupId = n.PolicyGroupId,
                        SupplierCode = n.SupplierCode,
                        SupplierName = n.SupplierName,
                        PolicyRoutingId = n.PolicyRoutingId,
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
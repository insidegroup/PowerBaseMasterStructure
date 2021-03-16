using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyMessageGroupItemCarRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Add
        public void Add(PolicyMessageGroupItemCar policyMessageGroupItemCar)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPolicyMessageGroupItemCar_v1(
                policyMessageGroupItemCar.PolicyGroupId,
                policyMessageGroupItemCar.PolicyMessageGroupItemName,
                policyMessageGroupItemCar.SupplierCode,
                policyMessageGroupItemCar.EnabledFlag,
                policyMessageGroupItemCar.EnabledDate,
                policyMessageGroupItemCar.ExpiryDate,
                policyMessageGroupItemCar.TravelDateValidFrom,
                policyMessageGroupItemCar.TravelDateValidTo,
                policyMessageGroupItemCar.PolicyLocationId,
                adminUserGuid
            );
        }

        public void Edit(PolicyMessageGroupItemCar policyMessageGroupItemCar)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePolicyMessageGroupItemCar_v1(
                policyMessageGroupItemCar.PolicyMessageGroupItemId,
                policyMessageGroupItemCar.PolicyMessageGroupItemName,
                policyMessageGroupItemCar.SupplierCode,
                policyMessageGroupItemCar.EnabledFlag,
                policyMessageGroupItemCar.EnabledDate,
                policyMessageGroupItemCar.ExpiryDate,
                policyMessageGroupItemCar.TravelDateValidFrom,
                policyMessageGroupItemCar.TravelDateValidTo,
                policyMessageGroupItemCar.PolicyLocationId,
                adminUserGuid,
                policyMessageGroupItemCar.VersionNumber
            );
        }

        public void Delete(PolicyMessageGroupItemCar policyMessageGroupItemCar)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePolicyMessageGroupItemCar_v1(
                policyMessageGroupItemCar.PolicyMessageGroupItemId,
                adminUserGuid,
                policyMessageGroupItemCar.VersionNumber
            );
        }

        //Get one PolicyGroup
        public PolicyMessageGroupItemCar GetPolicyMessageGroupItemCar(int policyMessageGroupItemId)
        {

            return (from n in db.spDesktopDataAdmin_SelectPolicyMessageGroupItemCar_v1(policyMessageGroupItemId)
                    select new
                        PolicyMessageGroupItemCar
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class ServicingOptionItemValueRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //public List<ServicingOptionItemValue> GetServicingOptionServicingOptionItemValuesOLD(int servicingOptionId, string clientSubUnitGuid)
        //{
        //    var result = from n in db.ServicingOptionItemValues.Where(c => c.ServicingOptionId == servicingOptionId).OrderBy(c => c.ServicingOptionItemValue1)
        //                 select n;
        //    return result.ToList();
        //}

        public List<ServicingOptionItemValue> GetServicingOptionServicingOptionItemValues(int servicingOptionId, int servicingOptionGroupId)
        {
            return (from n in db.spDesktopDataAdmin_SelectServicingOptionAvailableServicingOptionItemValues_v1(servicingOptionId, servicingOptionGroupId)
                    select new
                        ServicingOptionItemValue
                        {
                            ServicingOptionId= (int)n.ServicingOptionId,
                            ServicingOptionItemValue1 = n.ServicingOptionItemValue
                        }).ToList();
        }

         public List<ServicingOptionItemValue> GetClientSubUnitServicingOptionServicingOptionItemValues(int servicingOptionId, string clientSubUnitGuid)
        {
            return (from n in db.spDDAWizard_SelectClientSubUnitAvailableServicingOptionItemValues_v1(servicingOptionId, clientSubUnitGuid)
                    select new
                        ServicingOptionItemValue
                        {
                            ServicingOptionId= (int)n.ServicingOptionId,
                            ServicingOptionItemValue1 = n.ServicingOptionItemValue
                        }).ToList();
        }
    }
}
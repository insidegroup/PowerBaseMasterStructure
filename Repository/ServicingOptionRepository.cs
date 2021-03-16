using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class ServicingOptionRepository
    {
        private ServicingOptionDC db = new ServicingOptionDC(Settings.getConnectionString());

        public IQueryable<ServicingOption> GetAllServicingOptions()
        {
            return db.ServicingOptions.OrderBy(t => t.ServicingOptionName);
        }

        //v1.04.1
        //This replaces GetAllServicingOptions for populating the drop down for Creating A ServicingOptionItem
        //Onle one instance of 'CDR Driving Account/FOP' (ServicingOption.ServicingOptionId = 180) per ClientSubUnit
        //Onle one instance of 'Conditions for CDR Driving Account/FOP' (ServicingOption.ServicingOptionId = 181) per ClientSubUnit
        //These items are only available to a ClientSubUnit group
        //In short, only show 180, 181 for clientSubUnit Groups, also - dont show the option if the group already has one
        public List<ServicingOption> GetAvailableServicingOptions(int servicingOptionGroupId)
        {
            HierarchyDC db2 = new HierarchyDC(Settings.getConnectionString());
            return (from n in db2.spDesktopDataAdmin_SelectServicingOptionGroupAvailableServicingOptions_v1(servicingOptionGroupId)
                    select new
                        ServicingOption
                        {
                            ServicingOptionId = n.ServicingOptionId,
                            ServicingOptionName = n.ServicingOptionName
                        }).ToList();
        }

        //similar to GetAvailableServicingOptions but we only have  CSU GUID
        public List<ServicingOption> GetAvailableServicingOptionsClientSubUnit(string clientSubUnitGuid)
        {
            HierarchyDC db2 = new HierarchyDC(Settings.getConnectionString());
            return (from n in db2.spDDAWizard_SelectClientSubUnitAvailableServicingOptions_v1(clientSubUnitGuid)
                    select new
                        ServicingOption
                        {
                            ServicingOptionId = n.ServicingOptionId,
                            ServicingOptionName = n.ServicingOptionName
                        }).ToList();
        }

        public ServicingOption GetServicingOption(int servicingOptionId)
        {
            return db.ServicingOptions.SingleOrDefault(c => c.ServicingOptionId == servicingOptionId);
        }

        //Get the 3 HFLF OPtions that exist for a groupId
        public List<ServicingOption> GetServicingOptionsHFLFSelectList(int servicingOptionGroupId)
        {
            HierarchyDC db2 = new HierarchyDC(Settings.getConnectionString());
            return (from n in db2.spDesktopDataAdmin_SelectServicingOptionsHFLF_v1(servicingOptionGroupId)
                    select new
                        ServicingOption
                        {
                            ServicingOptionId = n.ServicingOptionId,
                            ServicingOptionName = n.ServicingOptionName
                        }).ToList();
        }

		public List<string> GetServicingOptionDepartureTimeWindows()
		{
			return new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" }.ToList();
		}

		public List<string> GetServicingOptionArrivalTimeWindows()
        {
			return new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" }.ToList();
        }

		public List<string> GetServicingOptionMaximumStops()
        {
			return new[] { "0", "1", "2", "3" }.ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Repository
{
    public class TablesDomainHierarchyLevelRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //REMOVED - returns items without checking access rights
        //public List<spDesktopDataAdmin_SelectDomainHierarchyLevels_v1Result> GetDomainHierarchies(string domain)
        //{
        //    return db.spDesktopDataAdmin_SelectDomainHierarchyLevels_v1(domain).ToList();
        //}

        //Returns a List of Hierarchies for a Domain based on the AdminUsers Access Rights
        public List<spDesktopDataAdmin_SelectSystemUserDomainHierarchyLevels_v1Result> GetDomainHierarchies(string domain)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return db.spDesktopDataAdmin_SelectSystemUserDomainHierarchyLevels_v1(domain, adminUserGuid).ToList();
        }

        //Returns a List of Hierarchies for a Domain based on the AdminUsers Access Rights
        public List<SelectListItem> GetDomainHierarchiesForHierarchySearch(string domain)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectSystemUserDomainHierarchyLevels_v1(domain, adminUserGuid).ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (spDesktopDataAdmin_SelectSystemUserDomainHierarchyLevels_v1Result item in result)
            {
                if (item.HierarchyLevelTableName == "ClientTopUnit")
                {
                    items.Add(new SelectListItem
                    {
                        Text = "ClientTopUnit Name",
                        Value = "ClientTopUnitName"
                    });
                    items.Add(new SelectListItem
                    {
                        Text = "ClientTopUnit Guid",
                        Value = "ClientTopUnitGuid"
                    });
                }
				else if (item.HierarchyLevelTableName == "ClientSubUnit")
                {
                    items.Add(new SelectListItem
                    {
                        Text = "ClientSubUnit Name",
                        Value = "ClientSubUnitName"
                    });
                    items.Add(new SelectListItem
                    {
                        Text = "ClientSubUnit Guid",
                        Value = "ClientSubUnitGuid"
                    });
                }
				else if (item.HierarchyLevelTableName == "ClientAccount")
				{
					items.Add(new SelectListItem
					{
						Text = "ClientAccount Name",
						Value = "ClientAccountName"
					});
					items.Add(new SelectListItem
					{
						Text = "ClientAccount Number",
						Value = "ClientAccountNumber"
					});
				}
				else if (item.HierarchyLevelTableName == "ClientSubUnitTravelerType" && (domain == "Client Rules Group Administrator" || domain == "Business Rules Group Administrator" || domain == "PNR Output"))
				{
					items.Add(new SelectListItem
					{
						Text = "Client SubUnit Traveler Type",
						Value = "ClientSubUnitTravelerType"
					});
				}
				else if (item.HierarchyLevelTableName == "TravelerType" && (domain == "Client Rules Group Administrator" || domain == "Business Rules Group Administrator" || domain == "PNR Output"))
				{
					items.Add(new SelectListItem
					{
						Text = "Client Traveler Type",
						Value = "TravelerType"
					});
				}
				else if (item.HierarchyLevelTableName == "Team")
                {
                    items.Add(new SelectListItem
                    {
                        Text = "Team Name",
                        Value = "TeamName"
                    });
                }
				else if (item.HierarchyLevelTableName == "Country")
                {
                    items.Add(new SelectListItem
                    {
                        Text = "Country Name",
                        Value = "CountryName"
                    });
                    items.Add(new SelectListItem
                    {
                        Text = "Country Code",
                        Value = "CountryCode"
                    });
                }
				else if (item.HierarchyLevelTableName == "Location")
                {
                    items.Add(new SelectListItem
                    {
                        Text = "Location Name",
                        Value = "LocationName"
                    });
                }
            }

            return items;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class GDSRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public List<GDS> GetAllGDSs()
        {
			var result = from n in db.spDesktopDataAdmin_SelectGDSs_v1().OrderBy(x => x.GDSName)
                         select new GDS
                         {
                             GDSCode = n.GDSCode,
                             GDSName = n.GDSName
                         };

            return result.ToList();
        }

		public List<GDS> GetAllGDSsExceptALL()
		{
			var result = from n in db.spDesktopDataAdmin_SelectGDSs_v1().Where(c => c.GDSCode != "ALL").OrderBy(x => x.GDSName)
						 select new GDS
						 {
							 GDSCode = n.GDSCode,
							 GDSName = n.GDSName
						 };

			return result.ToList();
		}

		public List<GDS> GetClientProfileBuilderGDSs()
		{
			var result = from n in db.spDesktopDataAdmin_SelectGDSs_v1()
							 .Where(c => c.GDSName == "Amadeus" ||
										 c.GDSName == "Apollo"  ||
										 c.GDSName == "Galileo" ||
										 c.GDSName == "Sabre")
							 .OrderBy(c => c.GDSName)

						 select new GDS
						 {
							 GDSCode = n.GDSCode,
							 GDSName = n.GDSName
						 };

			return result.ToList();
		}

        public GDS GetGDS(string gdsCode)
        {
            return db.GDS.SingleOrDefault(c => c.GDSCode == gdsCode);
        }

        //public void EditForDisplay(GDS gds)
        //{
        //    gds.
        //    return db.GDS.SingleOrDefault(c => c.GDSCode == code);
        //}
    }
}

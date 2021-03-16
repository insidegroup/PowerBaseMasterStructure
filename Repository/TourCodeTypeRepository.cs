using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using System.Linq;

namespace CWTDesktopDatabase.Repository
{
	public class TourCodeTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());


        public IQueryable<TourCodeType> GetAllTourCodeTypes()
        {
            return db.TourCodeTypes.OrderBy(c => c.TourCodeTypeDescription);
        }

		public TourCodeType GetTourCodeType(int tourCodeTypeId)
		{
			return db.TourCodeTypes.SingleOrDefault(c => c.TourCodeTypeId == tourCodeTypeId);
		}

		public List<TourCodeType> GetTourCodeTypesForGDS(string gdsCode)
		{
			var result = from n in  db.spDesktopDataAdmin_SelectTourCodeTypesForGDS_v1(gdsCode)
						 select new TourCodeType
							{
								TourCodeTypeId = n.TourCodeTypeId,
								TourCodeTypeDescription = n.TourCodeTypeDescription
							};
			return result.ToList();
		}
    }
}
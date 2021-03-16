using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class CarTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public IQueryable<CarType> GetAllCarTypes()
        {
            return db.CarTypes.OrderBy("CarTypeDescription");
        }
        public CarType GetCarType(string carTypeCode)
        {
            return db.CarTypes.SingleOrDefault(c => c.CarTypeCode == carTypeCode);
        }
    }
}

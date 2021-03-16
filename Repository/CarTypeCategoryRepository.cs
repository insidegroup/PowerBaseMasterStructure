using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class CarTypeCategoryRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public IQueryable<CarTypeCategory> GetAllCarTypeCategories()
        {
            return db.CarTypeCategories.OrderBy("CarTypeCategoryName");
        }
        public CarTypeCategory GetCarTypeCategory(int carTypeCategoryId)
        {
            return db.CarTypeCategories.SingleOrDefault(c => c.CarTypeCategoryId == carTypeCategoryId);
        }
    }
}
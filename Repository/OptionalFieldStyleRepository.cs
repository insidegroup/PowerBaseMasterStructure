using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class OptionalFieldStyleRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public OptionalFieldStyle GetStyle(int? optionalFieldStyleId)
        {
            OptionalFieldStyle optionalFieldStyle = new OptionalFieldStyle();
            optionalFieldStyle = db.OptionalFieldStyles.SingleOrDefault(c => c.OptionalFieldStyleId == optionalFieldStyleId);
            return optionalFieldStyle;
        }

        //Returns a List of OptionalFieldStyles
        public List<SelectListItem> GetAllOptionalFieldStyles()
        {
            var result = db.OptionalFieldStyles.OrderBy(c => c.OptionalFieldStyleDescription);
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (OptionalFieldStyle item in result)
            {
                items.Add(new SelectListItem
                {
                    Text = item.OptionalFieldStyleDescription,
                    Value = item.OptionalFieldStyleId.ToString()
                });

            }
            return items;

        }

		public IQueryable<OptionalFieldStyle> GetAllOptionalFieldStylesQueryable()
		{
			return db.OptionalFieldStyles.OrderBy(c => c.OptionalFieldStyleDescription);
		}
    }
}
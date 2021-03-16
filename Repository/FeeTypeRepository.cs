using System.Linq;
using CWTDesktopDatabase.Models;
using System.Web.Mvc;
using System.Collections.Generic;

namespace CWTDesktopDatabase.Repository
{
    public class FeeTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public FeeType GetFeeType(int feeTypeId)
        {
            FeeType feeType = new FeeType();
            feeType = db.FeeTypes.SingleOrDefault(c => c.FeeTypeId == feeTypeId);
            if (feeType != null)
            {
                if (feeType.FeeTypeDescription == "Client Fee")
                {
                    feeType.FeeTypeDescription = "Transaction Fee";
                }
            }
            return feeType;
        }

   

        //Returns a List of FeeTypes
        public List<SelectListItem> GetAllFeeTypes()
        {
            var result = db.FeeTypes.OrderBy(c => c.FeeTypeDescription);
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (FeeType item in result)
            {
                if (item.FeeTypeDescription == "Client Fee")
                {
                    items.Add(new SelectListItem
                    {
                        Text = "Transaction Fee",
                        Value = item.FeeTypeId.ToString()
                    });
                }
                else
                {
                    items.Add(new SelectListItem
                    {
                        Text = item.FeeTypeDescription,
                        Value = item.FeeTypeId.ToString()
                    });
                }

            }
            return items;

        }
    }
}

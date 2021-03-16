using System.Collections.Generic;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Repository
{
	public class CommonRepository
    {
        //Used for a True/False dropdown which maps to a boolean field
        public List<SelectListItem> GetTrueFalseList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem() { Value = "false", Text = "No" },
                new SelectListItem() { Value = "true", Text = "Yes" }
            };
        }

        //Used for a Allowed/Not Allowed dropdown which maps to a boolean field
        public List<SelectListItem> GetAllowedNotAllowedList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem() { Value = "true", Text = "Allowed" },
                new SelectListItem() { Value = "false", Text = "Not Allowed" }
            };
        }
    }
}

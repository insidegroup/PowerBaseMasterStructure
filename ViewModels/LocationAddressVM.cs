using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    
    public class AddressLocationVM : CWTBaseViewModel
    {
        public Location Location { get; set; }
        public Address Address { get; set; }
        public IEnumerable<SelectListItem> StateProvinces { get; set; }

        public AddressLocationVM()
        {
        }
		public AddressLocationVM(Location location, Address address)
        {
            Location = location;
            Address = address;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientTopUnitAddressVM : CWTBaseViewModel
   {
        public ClientTopUnit ClientTopUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public Address Address { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }
        public IEnumerable<SelectListItem> MappingQualityCodes { get; set; }

        public ClientTopUnitAddressVM()
        {
        }
        public ClientTopUnitAddressVM(ClientTopUnit clientTopUnit, ClientDetail clientDetail, Address address, IEnumerable<SelectListItem> countries, IEnumerable<SelectListItem> mappingQualityCodes)
        {
            ClientTopUnit = clientTopUnit;
            ClientDetail = clientDetail;
            Address = address;
            Countries = countries;
            MappingQualityCodes = mappingQualityCodes;
        }
    }
}
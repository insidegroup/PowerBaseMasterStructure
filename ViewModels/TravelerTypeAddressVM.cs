using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class TravelerTypeAddressVM : CWTBaseViewModel
     {
        public TravelerType TravelerType { get; set; }
        public ClientSubUnit ClientSubUnit { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public Address Address { get; set; }
        public IEnumerable<SelectListItem> MappingQualityCodes { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }

        public TravelerTypeAddressVM()
        {
        }
        public TravelerTypeAddressVM(TravelerType travelerType, ClientSubUnit clientSubUnit, ClientDetail clientDetail, Address address, IEnumerable<SelectListItem> mappingQualityCodes, IEnumerable<SelectListItem> countries)
        {
             TravelerType = travelerType;
            ClientSubUnit = clientSubUnit;
            ClientDetail = clientDetail;
            Address = address;
            MappingQualityCodes = mappingQualityCodes;
            Countries = countries;
        }
    }
}
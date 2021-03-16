using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientAccountAddressVM : CWTBaseViewModel
    {
        public ClientAccount ClientAccount { get; set; }
        public ClientDetail ClientDetail { get; set; }
        public Address Address { get; set; }
        public IEnumerable<SelectListItem> MappingQualityCodes { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }

        public ClientAccountAddressVM()
        {
        }
        public ClientAccountAddressVM(ClientAccount clientAccount, ClientDetail clientDetail, Address address, IEnumerable<SelectListItem> mappingQualityCodes, IEnumerable<SelectListItem> countries)
        {
            ClientAccount = clientAccount;
            ClientDetail = clientDetail;
            Address = address;
            MappingQualityCodes = mappingQualityCodes;
            Countries = countries;
        }
    }
}
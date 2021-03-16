using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientFeeLanguagesVM : CWTBaseViewModel
    {
        public ClientFee ClientFee { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeLanguageDescription_v1Result> ClientFeeLanguages { get; set; }
        
        public ClientFeeLanguagesVM()
        {
        }
        public ClientFeeLanguagesVM(ClientFee clientFee, CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeLanguageDescription_v1Result> clientFeeLanguages)
        {
            ClientFee = clientFee;
            ClientFeeLanguages = clientFeeLanguages;
        }
    }
}
using System.Collections.Generic;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientFeeVM : CWTBaseViewModel
   {
        public ClientFee ClientFee { get; set; }
        public ClientFeeOutput ClientFeeOutput { get; set; }
        public IEnumerable<SelectListItem> FeeTypes { get; set; }
        public IEnumerable<SelectListItem> GDSs { get; set; }
        public IEnumerable<SelectListItem> Contexts { get; set; }

        public ClientFeeVM()
        {
        }
        public ClientFeeVM(
                        ClientFee clientFee,
                        ClientFeeOutput clientFeeOutput,
                        IEnumerable<SelectListItem> contexts,
                        IEnumerable<SelectListItem> feeTypes
                        )
        {
            ClientFee = clientFee;
            ClientFeeOutput = clientFeeOutput;
            Contexts = contexts;
            FeeTypes = feeTypes;
        }
       
    }
}
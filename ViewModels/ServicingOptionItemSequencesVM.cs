using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class ServicingOptionItemSequencesVM : CWTBaseViewModel
   {
        public CWTPaginatedList<spDesktopDataAdmin_SelectServicingOptionItemSequences_v1Result> ServicingOptionItemSequences { get; set; }
        public ServicingOptionGroup ServicingOptionGroup { get; set; }
        public ServicingOption ServicingOption { get; set; }
        public int ServicingOptionId { get; set; }
 
        public ServicingOptionItemSequencesVM()
        {
        }
        public ServicingOptionItemSequencesVM(CWTPaginatedList<spDesktopDataAdmin_SelectServicingOptionItemSequences_v1Result> servicingOptionItemSequences, ServicingOptionGroup servicingOptionGroup, int servicingOptionId, ServicingOption servicingOption)
        {
            ServicingOptionItemSequences = servicingOptionItemSequences;
            ServicingOptionGroup = servicingOptionGroup;
            ServicingOption = servicingOption;
            ServicingOptionId = servicingOptionId;
        }
    }
}
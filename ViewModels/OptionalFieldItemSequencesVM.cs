using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class OptionalFieldItemSequencesVM : CWTBaseViewModel
   {
        public CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFieldItemSequences_v1Result> OptionalFieldItemSequences { get; set; }
        public OptionalFieldGroup OptionalFieldGroup { get; set; }
        public OptionalField OptionalField { get; set; }
        public int OptionalFieldId { get; set; }

        public OptionalFieldItemSequencesVM()
        {
        }
        public OptionalFieldItemSequencesVM(CWTPaginatedList<spDesktopDataAdmin_SelectOptionalFieldItemSequences_v1Result> servicingOptionItemSequences, OptionalFieldGroup servicingOptionGroup, int servicingOptionId, OptionalField servicingOption)
        {
            OptionalFieldItemSequences = servicingOptionItemSequences;
            OptionalFieldGroup = servicingOptionGroup;
            OptionalField = servicingOption;
            OptionalFieldId = servicingOptionId;
        }
    }
}
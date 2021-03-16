using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class PolicyCityGroupItemSequencesVM : CWTBaseViewModel
    {
        public PolicyGroup PolicyGroup { get; set; }
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCityGroupItemSequences_v1Result> Sequences { get; set; }
        
        public PolicyCityGroupItemSequencesVM()
        {
        }
        public PolicyCityGroupItemSequencesVM(PolicyGroup policyGroup, CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCityGroupItemSequences_v1Result> sequences)
        {
            PolicyGroup = policyGroup;
            Sequences = sequences;
        }
    }
}
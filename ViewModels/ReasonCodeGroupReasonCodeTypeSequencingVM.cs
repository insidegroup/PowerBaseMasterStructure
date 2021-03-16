using System.Collections.Generic;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Models
{
    public class ReasonCodeGroupReasonCodeTypeSequencingVM : CWTDesktopDatabase.ViewModels.CWTBaseViewModel
   {
        public CWTPaginatedList<spDesktopDataAdmin_SelectReasonCodeGroupReasonCodeTypeSequences_v1Result> SequenceItems { get; set; }
        public ReasonCodeGroup ReasonCodeGroup { get; set; }
        public int ReasonCodeTypeId  { get; set; }
        public int Page  { get; set; }
        public ReasonCodeGroupReasonCodeTypeSequencingVM()
        {
        }
        public ReasonCodeGroupReasonCodeTypeSequencingVM(ReasonCodeGroup reasonCodeGroup, int reasonCodeTypeId, int page, CWTPaginatedList<spDesktopDataAdmin_SelectReasonCodeGroupReasonCodeTypeSequences_v1Result> sequenceItems)
        {
            Page = page;
            ReasonCodeTypeId = reasonCodeTypeId;
            ReasonCodeGroup = reasonCodeGroup;
            SequenceItems = sequenceItems;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Validation;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.ViewModels
{
    [MetadataType(typeof(PolicyMessageGroupItemCreateVMValidation))]
    public class PolicyMessageGroupItemCreateVM : CWTBaseViewModel
    {
        public PolicyGroup PolicyGroup { get; set; }
        public GenericSelectListVM PolicyMessageGroupItemTypeSelectList { get; set; }
        public string PolicyMessageGroupItemType { get; set; }

        public PolicyMessageGroupItemCreateVM()
        {
        }
        public PolicyMessageGroupItemCreateVM(PolicyGroup policyGroup, GenericSelectListVM policyMessageGroupItemTypeSelectList, string policyMessageGroupItemType)
        {
            PolicyGroup = policyGroup;
            PolicyMessageGroupItemTypeSelectList = policyMessageGroupItemTypeSelectList;
            PolicyMessageGroupItemType = policyMessageGroupItemType;
        }
    }
}
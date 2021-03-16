using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyMessageGroupItemCreateVMValidation
    {
        [Required(ErrorMessage = "Type Required")]
        public string PolicyMessageGroupItemType { get; set; }
    }
}
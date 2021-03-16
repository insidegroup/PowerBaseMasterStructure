using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ApprovalGroupApprovalTypeValidation
    {
        [Required(ErrorMessage = "Approval Type ID Required")]
        [RegularExpression(@"^([1-9][0-9]{0,2})$", ErrorMessage = "Character entered is not allowed")]
        public int ApprovalGroupApprovalTypeId { get; set; }

        //Allowable special characters are alphanumeric, all accented characters and allowed special characters 
        //forward slash (/), asterisk (*), dash (-), underscore (_), space, period (.) and right and left parenthesis (())
        [RegularExpression(@"^([À-ÿ\w\s\(\)\-_\.\/\*]+)$", ErrorMessage = "Special character entered is not allowed")]
        [Required(ErrorMessage = "Approval Type Required")]
        public string ApprovalGroupApprovalTypeDescription { get; set; }
    }
}
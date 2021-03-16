using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class AdministratorRoleHierarchyLevelTypeValidation
    {
        [Required(ErrorMessage = "Role Required")]
        public string AdministratorRoleHierarchyLevelTypeName { get; set; }
    }
}

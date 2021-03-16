using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Validation
{
	[Bind(Exclude = "CreationTimestamp")]
	public class ClientTopUnitMatrixDPCodeValidation
    {
		[Required(ErrorMessage = "Matrix DP Code Required")]
		[RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "Special character entered is not allowed")]
		public string MatrixDPCode { get; set; }

        [Required(ErrorMessage = "Hierarchy Type Required")]
        public string HierarchyType { get; set; }
    }
}
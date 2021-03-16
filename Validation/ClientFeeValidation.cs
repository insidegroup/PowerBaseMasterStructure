using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ClientFeeValidation
    {
        [Required(ErrorMessage = "Description Required")]
        public string ClientFeeDescription { get; set; }

        [Required(ErrorMessage = "Fee Type Required")]
        public int FeeTypeId { get; set; }

        [Required(ErrorMessage = "GDS Required")]
        public int GDSCode { get; set; }

        [Required(ErrorMessage = "Context Required")]
        public int ContextId { get; set; }
    }
}
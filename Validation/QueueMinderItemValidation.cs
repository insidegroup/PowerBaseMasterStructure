using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class QueueMinderItemValidation
    {
        [Required(ErrorMessage = "Item Description Required")]
        public string QueueMinderItemDescription { get; set; }

        [Required(ErrorMessage = "Type Required")]
        public int QueueMinderTypeId { get; set; }

    }
}

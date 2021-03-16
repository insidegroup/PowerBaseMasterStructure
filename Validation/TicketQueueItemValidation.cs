
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class TicketQueueItemValidation
    {
        [Required(ErrorMessage = "TripType Required")]
        public string TicketTypeId { get; set; }

        [Required(ErrorMessage = "Description Required")]
        public string TicketQueueItemDescription { get; set; }

        [Required(ErrorMessage = "GDS Required")]
        public string GDSCode { get; set; }

		public string TicketingFieldRemark { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VenueTracker.Models
{
    [Table("tShow")]
    public class tShow
    {
        [Key]
        public int ShowId { get; set; }

        [Required]
        [ForeignKey("tStatus")]
        public int StatusId { get; set; }

        public tStatus? tStatus { get; set; }

        [Required]
        [ForeignKey("tVenue")]
        public int VenueId { get; set; }

        public tVenue? tVenue { get; set; }

        [Required(ErrorMessage = "Show date is required.")]
        public DateTime ShowDate { get; set; }

        [StringLength(200, ErrorMessage = "Show name cannot exceed 200 characters.")]
        public string? ShowName { get; set; }

        public decimal WalkAmount { get; set; }

        [ForeignKey("tPaymentType")]
        public int? PaymentTypeId { get; set; }

        public tPaymentType? tPaymentType { get; set; }

        public decimal? MerchAmount { get; set; }

        public string? Deal { get; set; }

        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters.")]
        public string? Notes { get; set; }

        public ICollection<tShowSubcontractor>? tShowSubcontractors { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}
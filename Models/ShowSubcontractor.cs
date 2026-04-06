using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VenueTracker.Models
{
    [Table("tShowSubcontractor")]
    public class tShowSubcontractor
    {
        [Key]
        public int ShowSubcontractorId { get; set; }

        [Required]
        [ForeignKey("tSubcontractor")]
        public int SubcontractorId { get; set; }

        public tSubcontractor? tSubcontractor { get; set; }

        [Required]
        [ForeignKey("tShow")]
        public int ShowId { get; set; }

        public tShow? tShow { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        public decimal Amount { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}

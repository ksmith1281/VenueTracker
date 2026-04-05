using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VenueTracker.Models
{
    public class Show
    {
        [Key]
        public int ShowId { get; set; }

        [Required]
        [ForeignKey("Venue")]
        public int VenueId { get; set; }

        public Venue? Venue { get; set; }

        [Required(ErrorMessage = "Show date is required.")]
        public DateTime ShowDate { get; set; }

        [StringLength(200, ErrorMessage = "Show name cannot exceed 200 characters.")]
        public string? ShowName { get; set; }

        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string? Status { get; set; }

        public string? Deal { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}
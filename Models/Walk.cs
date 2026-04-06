using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VenueTracker.Models
{
    public class Walk
    {
        [Key]
        public int WalkId { get; set; }

        [Required]
        [ForeignKey("Show")]
        public int ShowId { get; set; }

        public tShow? Show { get; set; }

        [Required]
        public decimal WalkAmount { get; set; }

        public decimal? MerchAmount { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}
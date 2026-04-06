using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VenueTracker.Models
{
    [Table("tStatus")]
    public class tStatus
    {
        [Key]
        public int StatusId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Status name cannot exceed 50 characters.")]
        public string? StatusName { get; set; }

        public ICollection<tShow>? tShows { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace VenueTracker.Models
{
    public class Venue
    {
        public int VenueId { get; set; }

        [Required(ErrorMessage = "Venue name is required.")]
        [StringLength(200, ErrorMessage = "Venue name cannot exceed 200 characters.")]
        public string? VenueName { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
        public string? City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Use 2-letter state abbreviation.")]
        public string? State { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Show>? Shows { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}
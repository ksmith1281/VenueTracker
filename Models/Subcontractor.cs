using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VenueTracker.Models
{
    [Table("tSubcontractor")]
    public class tSubcontractor
    {
        public int SubcontractorId { get; set; }

        [Required(ErrorMessage = "Subcontractor name is required.")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
        public string? Name { get; set; }

        [StringLength(100, ErrorMessage = "Role cannot exceed 100 characters.")]
        public string? Role { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(20, ErrorMessage = "Phone cannot exceed 20 characters.")]
        public string? Phone { get; set; }

        public decimal? PayPercentage { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}

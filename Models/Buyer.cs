using System.ComponentModel.DataAnnotations;

namespace VenueTracker.Models
{
    public class Buyer
    {
        public int BuyerId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string? LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(20, ErrorMessage = "Phone cannot exceed 20 characters.")]
        public string? Phone { get; set; }

        [Phone(ErrorMessage = "Invalid cell phone number.")]
        [StringLength(20, ErrorMessage = "Cell cannot exceed 20 characters.")]
        public string? Cell { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}

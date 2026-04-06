using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VenueTracker.Models
{
    [Table("tPaymentType")]
    public class tPaymentType
    {
        [Key]
        public int PaymentTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentType { get; set; } = string.Empty;

        public ICollection<tShow>? tShows { get; set; }
    }
}

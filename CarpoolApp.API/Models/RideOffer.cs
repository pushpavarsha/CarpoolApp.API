using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarpoolApp.API.Models
{
    public class RideOffer
    {
        [Key]
        public int RideOfferId { get; set; }

        [ForeignKey("Employee")]
        public int DriverId { get; set; }
        public Employee Driver { get; set; }

        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        [Required, MaxLength(150)]
        public string SourceLocation { get; set; }

        [Required, MaxLength(150)]
        public string DestinationLocation { get; set; }

        [Required]
        public DateTime RideDate { get; set; }

        [Required]
        public TimeSpan RideTime { get; set; }

        [Required]
        public int AvailableSeats { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "Active"; // Active, Completed, Cancelled

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<RideRequest> RideRequests { get; set; }
    }
}

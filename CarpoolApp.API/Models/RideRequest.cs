using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarpoolApp.API.Models
{
    public class RideRequest
    {
        [Key]
        public int RideRequestId { get; set; }

        [ForeignKey("RideOffer")]
        public int RideOfferId { get; set; }
        public RideOffer RideOffer { get; set; }

        [ForeignKey("Employee")]
        public int RequesterId { get; set; }
        public Employee Requester { get; set; }

        [ForeignKey("Status")]
        public int StatusId { get; set; }
        public RideRequestStatus Status { get; set; }

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RespondedAt { get; set; }
    }
}

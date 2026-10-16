using System.ComponentModel.DataAnnotations;

namespace CarpoolApp.API.Models
{
    public class RideRequestStatus
    {
        [Key]
        public int StatusId { get; set; }

        [Required, MaxLength(20)]
        public string StatusName { get; set; }   // Pending, Approved, Rejected, Completed

        public ICollection<RideRequest> RideRequests { get; set; }
    }
}

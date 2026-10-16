namespace CarpoolApp.API.DTO
{
    public class RideRequestCreateDto
    {
        public int RideOfferId { get; set; }
        public int RequesterId { get; set; }
    }

    public class RideRequestResponseDto
    {
        public int RideRequestId { get; set; }
        public int DriverId { get; set; }
        public int RideOfferId { get; set; }
        public string SourceLocation { get; set; }
        public string DestinationLocation { get; set; }
        public DateTime RideDate { get; set; }
        public int RequesterId { get; set; }
        public string RequesterName { get; set; }
        public string Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? RespondedAt { get; set; }
    }

    public class RideRequestStatusUpdateDto
    {
        public string Status { get; set; } // Approved, Rejected, Completed
    }
}

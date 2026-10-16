namespace CarpoolApp.API.DTO
{
    public class RideOfferCreateDto
    {
        public int DriverId { get; set; }
        public int VehicleId { get; set; }
        public string SourceLocation { get; set; }
        public string DestinationLocation { get; set; }
        public DateTime RideDate { get; set; }
        public TimeSpan RideTime { get; set; }
        public int AvailableSeats { get; set; }
    }

    public class RideOfferResponseDto
    {
        public int RideOfferId { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public int VehicleId { get; set; }
        public string VehicleModel { get; set; }
        public string SourceLocation { get; set; }
        public string DestinationLocation { get; set; }
        public DateTime RideDate { get; set; }
        public TimeSpan RideTime { get; set; }
        public int AvailableSeats { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class RideOfferUpdateDto
    {
        public string SourceLocation { get; set; }
        public string DestinationLocation { get; set; }
        public DateTime RideDate { get; set; }
        public TimeSpan RideTime { get; set; }
        public int AvailableSeats { get; set; }
        public string Status { get; set; }
    }

}

namespace CarpoolApp.API.DTO
{
    public class VehicleCreateDto
    {
        public int EmployeeId { get; set; }
        public string RegistrationNumber { get; set; }
        public string Model { get; set; }
        public int SeatCapacity { get; set; }
    }

    public class VehicleResponseDto
    {
        public int VehicleId { get; set; }
        public int EmployeeId { get; set; }
        public string OwnerName { get; set; }
        public string RegistrationNumber { get; set; }
        public string Model { get; set; }
        public int SeatCapacity { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class VehicleUpdateDto
    {
        public string RegistrationNumber { get; set; }
        public string Model { get; set; }
        public int SeatCapacity { get; set; }
    }
}

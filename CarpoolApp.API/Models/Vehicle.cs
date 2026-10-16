using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CarpoolApp.API.Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleId { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Required, MaxLength(20)]
        public string RegistrationNumber { get; set; }

        [Required, MaxLength(50)]
        public string Model { get; set; }

        [Required]
        public int SeatCapacity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

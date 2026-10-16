namespace CarpoolApp.API.DTO
{// Used when registering a new employee
    public class EmployeeCreateDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
    }

    // Used when returning employee data to the client (no password)
    public class EmployeeResponseDto
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Used when updating employee details
    public class EmployeeUpdateDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
    }

    // Used for login
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

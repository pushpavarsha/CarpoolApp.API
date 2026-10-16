using CarpoolApp.API.DTO;

namespace CarpoolApp.API.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeResponseDto> RegisterAsync(EmployeeCreateDto dto);
        Task<EmployeeResponseDto> GetByIdAsync(int id);
        Task<(List<EmployeeResponseDto> Items, int TotalCount)> SearchAsync(string searchTerm, int pageNumber, int pageSize);
        Task<EmployeeResponseDto> UpdateAsync(int id, EmployeeUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<EmployeeResponseDto> LoginAsync(LoginDto dto);
    }
}

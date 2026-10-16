using CarpoolApp.API.DTO;

namespace CarpoolApp.API.Services
{
    public interface IVehicleService
    {
        Task<VehicleResponseDto> AddAsync(VehicleCreateDto dto);
        Task<VehicleResponseDto> GetByIdAsync(int id);
        Task<(List<VehicleResponseDto> Items, int TotalCount)> SearchAsync(string searchTerm, int pageNumber, int pageSize);
        Task<VehicleResponseDto> UpdateAsync(int id, VehicleUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

using CarpoolApp.API.DTO;

namespace CarpoolApp.API.Services
{
    public interface IRideRequestService
    {
        Task<RideRequestResponseDto> CreateAsync(RideRequestCreateDto dto);
        Task<RideRequestResponseDto> GetByIdAsync(int id);
        Task<(List<RideRequestResponseDto> Items, int TotalCount)> SearchAsync(string searchTerm, int pageNumber, int pageSize);
        Task<RideRequestResponseDto> UpdateStatusAsync(int id, RideRequestStatusUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

using CarpoolApp.API.DTO;

namespace CarpoolApp.API.Services
{
    public interface IRideOfferService
    {
        Task<RideOfferResponseDto> CreateAsync(RideOfferCreateDto dto);
        Task<RideOfferResponseDto> GetByIdAsync(int id);
        Task<(List<RideOfferResponseDto> Items, int TotalCount)> SearchAsync(string searchTerm, int pageNumber, int pageSize);
        Task<RideOfferResponseDto> UpdateAsync(int id, RideOfferUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

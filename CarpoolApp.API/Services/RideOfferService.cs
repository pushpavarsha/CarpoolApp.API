using CarpoolApp.API.Data;
using CarpoolApp.API.DTO;
using CarpoolApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CarpoolApp.API.Services
{
    public class RideOfferService : IRideOfferService
    {
        private readonly CarpoolDbContext _context;

        public RideOfferService(CarpoolDbContext context)
        {
            _context = context;
        }

        public async Task<RideOfferResponseDto> CreateAsync(RideOfferCreateDto dto)
        {
            var driver = await _context.Employees.FindAsync(dto.DriverId);
            if (driver == null)
                throw new InvalidOperationException("Driver not found.");

            var vehicle = await _context.Vehicles.FindAsync(dto.VehicleId);
            if (vehicle == null)
                throw new InvalidOperationException("Vehicle not found.");

            if (vehicle.EmployeeId != dto.DriverId)
                throw new InvalidOperationException("This vehicle does not belong to the selected driver.");

            if (dto.AvailableSeats > vehicle.SeatCapacity)
                throw new InvalidOperationException("Available seats cannot exceed vehicle seat capacity.");

            var rideOffer = new RideOffer
            {
                DriverId = dto.DriverId,
                VehicleId = dto.VehicleId,
                SourceLocation = dto.SourceLocation,
                DestinationLocation = dto.DestinationLocation,
                RideDate = dto.RideDate,
                RideTime = dto.RideTime,
                AvailableSeats = dto.AvailableSeats,
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            };

            _context.RideOffers.Add(rideOffer);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(rideOffer.RideOfferId);
        }

        public async Task<RideOfferResponseDto> GetByIdAsync(int id)
        {
            var offer = await _context.RideOffers
                .Include(r => r.Driver)
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(r => r.RideOfferId == id);

            return offer == null ? null : MapToDto(offer);
        }

        public async Task<(List<RideOfferResponseDto> Items, int TotalCount)> SearchAsync(string searchTerm, int pageNumber, int pageSize)
        {
            var query = _context.RideOffers
                .Include(r => r.Driver)
                .Include(r => r.Vehicle)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(r =>
                    r.SourceLocation.Contains(searchTerm) ||
                    r.DestinationLocation.Contains(searchTerm) ||
                    r.Driver.FullName.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(r => r.RideDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items.Select(MapToDto).ToList(), totalCount);
        }

        public async Task<RideOfferResponseDto> UpdateAsync(int id, RideOfferUpdateDto dto)
        {
            var offer = await _context.RideOffers.FindAsync(id);
            if (offer == null) return null;

            offer.SourceLocation = dto.SourceLocation;
            offer.DestinationLocation = dto.DestinationLocation;
            offer.RideDate = dto.RideDate;
            offer.RideTime = dto.RideTime;
            offer.AvailableSeats = dto.AvailableSeats;
            offer.Status = dto.Status;

            await _context.SaveChangesAsync();
            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var offer = await _context.RideOffers.FindAsync(id);
            if (offer == null) return false;

            _context.RideOffers.Remove(offer);
            await _context.SaveChangesAsync();
            return true;
        }

        private static RideOfferResponseDto MapToDto(RideOffer r)
        {
            return new RideOfferResponseDto
            {
                RideOfferId = r.RideOfferId,
                DriverId = r.DriverId,
                DriverName = r.Driver != null ? r.Driver.FullName : null,
                VehicleId = r.VehicleId,
                VehicleModel = r.Vehicle != null ? r.Vehicle.Model : null,
                SourceLocation = r.SourceLocation,
                DestinationLocation = r.DestinationLocation,
                RideDate = r.RideDate,
                RideTime = r.RideTime,
                AvailableSeats = r.AvailableSeats,
                Status = r.Status,
                CreatedAt = r.CreatedAt
            };
        }
    }
}

using CarpoolApp.API.Data;
using CarpoolApp.API.DTO;
using CarpoolApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CarpoolApp.API.Services
{
    public class RideRequestService : IRideRequestService
    {
        private readonly CarpoolDbContext _context;

        // Fixed status IDs, matching the seeded data
        private const int Pending = 1;
        private const int Approved = 2;
        private const int Rejected = 3;
        private const int Completed = 4;

        public RideRequestService(CarpoolDbContext context)
        {
            _context = context;
        }

        public async Task<RideRequestResponseDto> CreateAsync(RideRequestCreateDto dto)
        {
            var rideOffer = await _context.RideOffers.FindAsync(dto.RideOfferId);
            if (rideOffer == null)
                throw new InvalidOperationException("Ride offer not found.");

            if (rideOffer.Status != "Active")
                throw new InvalidOperationException("This ride is no longer active.");

            if (rideOffer.AvailableSeats <= 0)
                throw new InvalidOperationException("No seats available on this ride.");

            if (rideOffer.DriverId == dto.RequesterId)
                throw new InvalidOperationException("Driver cannot request their own ride.");

            var alreadyRequested = await _context.RideRequests.AnyAsync(rr =>
                rr.RideOfferId == dto.RideOfferId &&
                rr.RequesterId == dto.RequesterId &&
                rr.StatusId != Rejected);

            if (alreadyRequested)
                throw new InvalidOperationException("You have already requested this ride.");

            var rideRequest = new RideRequest
            {
                RideOfferId = dto.RideOfferId,
                RequesterId = dto.RequesterId,
                StatusId = Pending,
                RequestedAt = DateTime.UtcNow
            };

            _context.RideRequests.Add(rideRequest);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(rideRequest.RideRequestId);
        }

        public async Task<RideRequestResponseDto> GetByIdAsync(int id)
        {
            var request = await _context.RideRequests
                .Include(rr => rr.RideOffer)
                .Include(rr => rr.Requester)
                .Include(rr => rr.Status)
                .FirstOrDefaultAsync(rr => rr.RideRequestId == id);

            return request == null ? null : MapToDto(request);
        }

        public async Task<(List<RideRequestResponseDto> Items, int TotalCount)> SearchAsync(string searchTerm, int pageNumber, int pageSize)
        {
            var query = _context.RideRequests
                .Include(rr => rr.RideOffer)
                .Include(rr => rr.Requester)
                .Include(rr => rr.Status)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(rr =>
                    rr.Requester.FullName.Contains(searchTerm) ||
                    rr.RideOffer.SourceLocation.Contains(searchTerm) ||
                    rr.RideOffer.DestinationLocation.Contains(searchTerm) ||
                    rr.Status.StatusName.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(rr => rr.RequestedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items.Select(MapToDto).ToList(), totalCount);
        }

        public async Task<RideRequestResponseDto> UpdateStatusAsync(int id, RideRequestStatusUpdateDto dto)
        {
            var request = await _context.RideRequests
                .Include(rr => rr.RideOffer)
                .FirstOrDefaultAsync(rr => rr.RideRequestId == id);

            if (request == null) return null;

            // Look up the StatusId that matches the status name sent in (e.g. "Approved")
            var targetStatus = await _context.RideRequestStatuses
                .FirstOrDefaultAsync(s => s.StatusName == dto.Status);

            if (targetStatus == null)
                throw new InvalidOperationException("Invalid status value.");

            if (targetStatus.StatusId == Approved)
            {
                if (request.RideOffer.AvailableSeats <= 0)
                    throw new InvalidOperationException("No seats available to approve this request.");

                request.RideOffer.AvailableSeats -= 1;
            }

            request.StatusId = targetStatus.StatusId;
            request.RespondedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var request = await _context.RideRequests.FindAsync(id);
            if (request == null) return false;

            _context.RideRequests.Remove(request);
            await _context.SaveChangesAsync();
            return true;
        }

        private static RideRequestResponseDto MapToDto(RideRequest rr)
        {
            return new RideRequestResponseDto
            {
                RideRequestId = rr.RideRequestId,
                DriverId = rr.RideOffer != null ? rr.RideOffer.DriverId : 0,
                RideOfferId = rr.RideOfferId,
                SourceLocation = rr.RideOffer?.SourceLocation,
                DestinationLocation = rr.RideOffer?.DestinationLocation,
                RideDate = rr.RideOffer != null ? rr.RideOffer.RideDate : default,
                RequesterId = rr.RequesterId,
                RequesterName = rr.Requester != null ? rr.Requester.FullName : null,
                Status = rr.Status != null ? rr.Status.StatusName : null,
                RequestedAt = rr.RequestedAt,
                RespondedAt = rr.RespondedAt
            };
        }
    }
}

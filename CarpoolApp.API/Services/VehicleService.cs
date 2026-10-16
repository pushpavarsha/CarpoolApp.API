using CarpoolApp.API.Data;
using CarpoolApp.API.DTO;
using CarpoolApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CarpoolApp.API.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly CarpoolDbContext _context;

        public VehicleService(CarpoolDbContext context)
        {
            _context = context;
        }

        public async Task<VehicleResponseDto> AddAsync(VehicleCreateDto dto)
        {
            var employeeExists = await _context.Employees.AnyAsync(e => e.EmployeeId == dto.EmployeeId);
            if (!employeeExists)
                throw new InvalidOperationException("Employee not found.");

            var vehicle = new Vehicle
            {
                EmployeeId = dto.EmployeeId,
                RegistrationNumber = dto.RegistrationNumber,
                Model = dto.Model,
                SeatCapacity = dto.SeatCapacity,
                CreatedAt = DateTime.UtcNow
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(vehicle.VehicleId);
        }

        public async Task<VehicleResponseDto> GetByIdAsync(int id)
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.Employee)
                .FirstOrDefaultAsync(v => v.VehicleId == id);

            return vehicle == null ? null : MapToDto(vehicle);
        }

        public async Task<(List<VehicleResponseDto> Items, int TotalCount)> SearchAsync(string searchTerm, int pageNumber, int pageSize)
        {
            var query = _context.Vehicles.Include(v => v.Employee).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(v =>
                    v.RegistrationNumber.Contains(searchTerm) ||
                    v.Model.Contains(searchTerm) ||
                    v.Employee.FullName.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(v => v.Model)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items.Select(MapToDto).ToList(), totalCount);
        }

        public async Task<VehicleResponseDto> UpdateAsync(int id, VehicleUpdateDto dto)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return null;

            vehicle.RegistrationNumber = dto.RegistrationNumber;
            vehicle.Model = dto.Model;
            vehicle.SeatCapacity = dto.SeatCapacity;

            await _context.SaveChangesAsync();
            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return false;

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return true;
        }

        private static VehicleResponseDto MapToDto(Vehicle v)
        {
            return new VehicleResponseDto
            {
                VehicleId = v.VehicleId,
                EmployeeId = v.EmployeeId,
                OwnerName = v.Employee != null ? v.Employee.FullName : null,
                RegistrationNumber = v.RegistrationNumber,
                Model = v.Model,
                SeatCapacity = v.SeatCapacity,
                CreatedAt = v.CreatedAt
            };
        }
    }
}

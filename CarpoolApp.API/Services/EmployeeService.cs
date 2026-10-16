using CarpoolApp.API.Data;
using CarpoolApp.API.DTO;
using CarpoolApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CarpoolApp.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly CarpoolDbContext _context;

        public EmployeeService(CarpoolDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeResponseDto> RegisterAsync(EmployeeCreateDto dto)
        {
            var exists = await _context.Employees.AnyAsync(e => e.Email == dto.Email);
            if (exists)
                throw new InvalidOperationException("An employee with this email already exists.");

            var employee = new Employee
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                PhoneNumber = dto.PhoneNumber,
                Department = dto.Department,
                CreatedAt = DateTime.UtcNow
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return MapToDto(employee);
        }

        public async Task<EmployeeResponseDto> GetByIdAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            return employee == null ? null : MapToDto(employee);
        }

        public async Task<(List<EmployeeResponseDto> Items, int TotalCount)> SearchAsync(string searchTerm, int pageNumber, int pageSize)
        {
            var query = _context.Employees.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e =>
                    e.FullName.Contains(searchTerm) ||
                    e.Email.Contains(searchTerm) ||
                    e.Department.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(e => e.FullName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EmployeeResponseDto
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FullName,
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,
                    Department = e.Department,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<EmployeeResponseDto> UpdateAsync(int id, EmployeeUpdateDto dto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return null;

            employee.FullName = dto.FullName;
            employee.PhoneNumber = dto.PhoneNumber;
            employee.Department = dto.Department;

            await _context.SaveChangesAsync();
            return MapToDto(employee);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<EmployeeResponseDto> LoginAsync(LoginDto dto)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == dto.Email);
            if (employee == null) return null;

            bool validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, employee.PasswordHash);
            if (!validPassword) return null;

            return MapToDto(employee);
        }

        private static EmployeeResponseDto MapToDto(Employee e)
        {
            return new EmployeeResponseDto
            {
                EmployeeId = e.EmployeeId,
                FullName = e.FullName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                Department = e.Department,
                CreatedAt = e.CreatedAt
            };
        }
    }
}

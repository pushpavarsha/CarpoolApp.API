using CarpoolApp.API.DTO;
using CarpoolApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarpoolApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        // POST: api/vehicle
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] VehicleCreateDto dto)
        {
            try
            {
                var result = await _vehicleService.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.VehicleId }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/vehicle/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _vehicleService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // GET: api/vehicle/search?searchTerm=Honda&pageNumber=1&pageSize=10
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? searchTerm=null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var (items, totalCount) = await _vehicleService.SearchAsync(searchTerm, pageNumber, pageSize);

            return Ok(new
            {
                items,
                totalCount,
                pageNumber,
                pageSize,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            });
        }

        // PUT: api/vehicle/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VehicleUpdateDto dto)
        {
            var result = await _vehicleService.UpdateAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // DELETE: api/vehicle/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _vehicleService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}

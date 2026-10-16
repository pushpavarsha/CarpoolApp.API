using CarpoolApp.API.DTO;
using CarpoolApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarpoolApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RideRequestController : ControllerBase
    {
        private readonly IRideRequestService _rideRequestService;

        public RideRequestController(IRideRequestService rideRequestService)
        {
            _rideRequestService = rideRequestService;
        }

        // POST: api/riderequest
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RideRequestCreateDto dto)
        {
            try
            {
                var result = await _rideRequestService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.RideRequestId }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/riderequest/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _rideRequestService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // GET: api/riderequest/search?searchTerm=Pending&pageNumber=1&pageSize=10
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? searchTerm=null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var (items, totalCount) = await _rideRequestService.SearchAsync(searchTerm, pageNumber, pageSize);

            return Ok(new
            {
                items,
                totalCount,
                pageNumber,
                pageSize,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            });
        }

        // PUT: api/riderequest/5/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] RideRequestStatusUpdateDto dto)
        {
            try
            {
                var result = await _rideRequestService.UpdateStatusAsync(id, dto);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/riderequest/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _rideRequestService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}

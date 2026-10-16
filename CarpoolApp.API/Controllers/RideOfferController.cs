using CarpoolApp.API.DTO;
using CarpoolApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarpoolApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RideOfferController : ControllerBase
    {
        private readonly IRideOfferService _rideOfferService;

        public RideOfferController(IRideOfferService rideOfferService)
        {
            _rideOfferService = rideOfferService;
        }

        // POST: api/rideoffer
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RideOfferCreateDto dto)
        {
            try
            {
                var result = await _rideOfferService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.RideOfferId }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/rideoffer/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _rideOfferService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // GET: api/rideoffer/search?searchTerm=Pune&pageNumber=1&pageSize=10
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? searchTerm=null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var (items, totalCount) = await _rideOfferService.SearchAsync(searchTerm, pageNumber, pageSize);

            return Ok(new
            {
                items,
                totalCount,
                pageNumber,
                pageSize,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            });
        }

        // PUT: api/rideoffer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RideOfferUpdateDto dto)
        {
            var result = await _rideOfferService.UpdateAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // DELETE: api/rideoffer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _rideOfferService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}

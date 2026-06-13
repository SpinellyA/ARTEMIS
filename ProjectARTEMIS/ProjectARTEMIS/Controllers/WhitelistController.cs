using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YourProject.Controllers
{
    [ApiController]
    [Route("api/v1/whitelists")]
    [Authorize(Roles = "Admin")] // Restricts all endpoints in this controller to Admin role
    public class WhitelistController : ControllerBase
    {
        private readonly WhitelistRequestService _requestService;

        public WhitelistController(WhitelistRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        public async Task<ActionResult<List<WhitelistApplicationDto>>> GetAll()
        {
            var requests = await _requestService.GetAllWhitelistRequestsAsync();
            return Ok(requests);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<WhitelistApplicationDto>> GetById(Guid id)
        {
            var request = await _requestService.GetByIdAsync(id);
            if (request == null)
            {
                return NotFound($"Whitelist request with ID {id} was not found.");
            }

            return Ok(request);
        }

        [HttpPost("request")]
        [AllowAnonymous] // Allows non-admins/guests to submit requests
        public async Task<IActionResult> CreateRequest([FromBody] RequestWhitelistDto dto)
        {
            await _requestService.RequestWhitelistAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("accept")]
        public async Task<IActionResult> AcceptRequest([FromBody] AcceptWhitelistRequestDto dto)
        {
            try
            {
                await _requestService.AcceptWhitelistRequestAsync(dto);
                return Ok(new { message = "Request accepted successfully." });
            }
            catch (ApplicationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("reject")]
        public async Task<IActionResult> RejectRequest([FromBody] AcceptWhitelistRequestDto dto)
        {
            try
            {
                await _requestService.RejectWhitelistRequestAsync(dto);
                return Ok(new { message = "Request rejected successfully." });
            }
            catch (ApplicationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

public record RequestWhitelistDto
{
    public Guid UserId { get; set; }
    public string RealName { get; set; } = string.Empty;
    public Guid SchoolId { get; set; }
    public string FacebookUrl { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class WhitelistApplicationDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string RealName { get; set; } = string.Empty;
    public string School { get; set; } = string.Empty;
    public string FacebookUrl { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public record AcceptWhitelistRequestDto
{
    public Guid WhitelistRequestId { get; set; }
}
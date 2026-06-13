using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace YourProjectNamespace.Controllers
{
    [ApiController]
    [Route("api/v1/players")]
    [Authorize]
    public class PlayerProfileController : ControllerBase
    {
        private readonly PlayerProfileService _profileService;
        private readonly IWebHostEnvironment _environment;

        public PlayerProfileController(PlayerProfileService profileService, IWebHostEnvironment environment)
        {
            _profileService = profileService;
            _environment = environment;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<PlayerProfileDto>> GetProfile(string username)
        {
            try
            {
                var profile = await _profileService.GetPlayerProfileAsync(username);
                return Ok(profile);
            }
            catch (Exception ex) when (ex is NullReferenceException || ex is KeyNotFoundException)
            {
                return NotFound(new { Message = $"Player profile for user '{username}' was not found." });
            }
        }

        [HttpPut("details")]
        public async Task<IActionResult> UpdateDetails([FromBody] UpdatePlayerProfileRequest request)
        {
            if (!UserOwnsProfile(request.Id))
            {
                return Forbid(); 
            }

            try
            {
                await _profileService.UpdateProfileDetails(request);
                return Ok(new { Message = "Profile details updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost("upload-picture")]
        public async Task<IActionResult> UploadProfilePicture([FromForm] UploadProfilePictureForm form)
        {
            // 👈 Ownership Check
            if (!UserOwnsProfile(form.Id))
            {
                return Forbid(); // Returns HTTP 403 Forbidden
            }

            if (form.ProfilePicture == null || form.ProfilePicture.Length == 0)
            {
                return BadRequest(new { Message = "No file stream was provided." });
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(form.ProfilePicture.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new { Message = "Invalid image extension." });
            }

            try
            {
                var folderName = Path.Combine(_environment.WebRootPath, "images", "profiles");
                if (!Directory.Exists(folderName)) Directory.CreateDirectory(folderName);

                var uniqueFileName = $"{form.Id}_{DateTime.UtcNow.Ticks}{extension}";
                var targetDiskPath = Path.Combine(folderName, uniqueFileName);

                using (var stream = new FileStream(targetDiskPath, FileMode.Create))
                {
                    await form.ProfilePicture.CopyToAsync(stream);
                }

                var webRoutePath = $"/images/profiles/{uniqueFileName}";
                var serviceRequest = new UploadNewProfilePictureRequest
                {
                    Id = form.Id,
                    ProfilePicturePath = webRoutePath
                };

                await _profileService.UpdateProfilePicture(serviceRequest);

                return Ok(new { Message = "Profile picture updated successfully.", Path = webRoutePath });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        private bool UserOwnsProfile(Guid targetId)
        {
            var nameIdentifierClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(nameIdentifierClaim) || !Guid.TryParse(nameIdentifierClaim, out var authenticatedUserId))
            {
                return false;
            }

            return authenticatedUserId == targetId;
        }
    }
}

public record UploadProfilePictureForm
{
    [FromForm(Name = "id")]
    public Guid Id { get; set; }

    [FromForm(Name = "profilePicture")]
    public IFormFile ProfilePicture { get; set; } = null!;
}

public record UpdatePlayerProfileRequest
{
    public Guid Id { get; set; }
    public string Bio { get; set; }
}
public record UploadNewProfilePictureRequest
{
    public Guid Id { get; set; }
    public string ProfilePicturePath { get; set; } = string.Empty;
}

public record PlayerProfileDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public SchoolDto School { get; set; }
    public string Bio { get; set; }
    public string Status { get; set; } // banned, whitelisted, etc
    public string OnlineStatus { get; set; } // online or offline
    public List<SocialDto> Socials { get; set; }
    public string ProfilePicturePath { get; set; } = string.Empty;
}

public record SocialDto
{
    public string SocialName { get; set; }
    public string Link { get; set; }
}
public record SchoolDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ColorCode { get; set; } = string.Empty;
}
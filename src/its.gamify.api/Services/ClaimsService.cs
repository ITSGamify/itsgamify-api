using System.Security.Claims;
using its.gamify.core.Services.Interfaces;

namespace its.gamify.api.Services;
public class ClaimsService : IClaimsService
{
    public ClaimsService(IHttpContextAccessor httpContextAccessor)
    {
        var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(claimType: ClaimTypes.NameIdentifier);
        CurrentUser = string.IsNullOrEmpty(userId) ? Guid.Empty : Guid.Parse(userId);
    }
    public Guid CurrentUser { get; }
}
using its.gamify.core.Features.AvailablesData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace its.gamify.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private Ultils data;
        public NotificationController(Ultils data)
        {
            this.data = data;
        }
        /// <summary>
        /// Get all notification
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 0,
                                        [FromQuery] int pageSize = 10,
                                        [FromQuery] string searchTerm = ""
                                        )
        {
            return Ok(data.notifications);

        }
    }
}

using its.gamify.core.Features.AvailablesData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace its.gamify.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private Ultils data;
        public QuestionController(Ultils data)
        {
            this.data = data;
        }
        /// <summary>
        /// Get all question
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 0,
                                        [FromQuery] int pageSize = 10,
                                        [FromQuery] string searchTerm = ""
                                        )
        {
            return Ok(data.questions);

        }
    }
}

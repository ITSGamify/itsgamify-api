using its.gamify.api.Features.Users.Queries;
using its.gamify.core.Features.AvailablesData;
using its.gamify.core.Models.Courses;
using its.gamify.core.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace its.gamify.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IMediator mediator;
        private Ultils data;
        public CourseController(ICourseService courseService, IMediator mediator, Ultils data)
        {
            _courseService = courseService;
            this.mediator = mediator;
            this.data = data;
        }
        /// <summary>
        /// Delete course
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _courseService.Delete(id);
            if (result) return Ok("Delete Successfully");
            else return BadRequest("Deleted Failed");
        }

        /// <summary>
        /// Get all course
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 0,
                                        [FromQuery] int pageSize = 10,
                                        [FromQuery] string searchTerm=""
                                        )
        {
            return Ok(data.courses);
            /*if (result.Count() > 0) { return Ok(result); }
            else return BadRequest();*/

        }

        /// <summary>
        /// Update course
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] CourseUpdateModel updatedItem)
        {
            var result = await _courseService.Update(updatedItem);
            if (result) return NoContent();
            else return BadRequest();
        }

        /// <summary>
        /// Create course
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CourseCreateModel createItem)
        {
            /*createcourseDTO.File = formFile;*/
            var result = await _courseService.Create(createItem);
            if (result is null)
            {
                return BadRequest("Can not create Course");

            }
            else return Ok(result);
        }


        /// <summary>
        /// Get course by Id
        /// </summary>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _courseService.GetCourse(id);
            return Ok(result);
        }
    }
}

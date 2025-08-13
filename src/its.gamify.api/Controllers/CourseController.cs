using its.gamify.api.Features.CourseParticipations;
using its.gamify.api.Features.Courses.Commands;
using its.gamify.api.Features.Courses.Queries;
using its.gamify.api.Features.CourseSections.Commands;
using its.gamify.api.Features.CourseSections.Queries;
using its.gamify.api.Features.LearningMaterials.Commands;
using its.gamify.api.Features.Users.Queries;
using its.gamify.core.Features.AvailablesData;
using its.gamify.core.Features.LearningMaterials.Queries;
using its.gamify.core.Models.Courses;
using its.gamify.core.Models.CourseSections;
using its.gamify.core.Models.LearningMaterials;
using its.gamify.core.Models.ShareModels;
using its.gamify.core.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace its.gamify.api.Controllers
{
    [Route("api/[controller]s")]
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
        public async Task<IActionResult> GetAll([FromQuery] FilterQuery query)
        {
            var res = await mediator.Send(new GetAllCourseQuery()
            {
                filterQuery = query
            });
            return Ok(res);
            /*if (result.Count() > 0) { return Ok(result); }
            else return BadRequest();*/

        }
        [HttpPost("{id}/course-sections")]
        public async Task<IActionResult> CreateCourseSection([FromForm] CourseSectionCreateModel command,

            [FromForm] List<IFormFile> files,
            [FromRoute] Guid id)
        {

            return Ok(await mediator.Send(new UpsertCourseSectionCommand()
            {
                //CourseId = id,
                //Lessons = command.Lessons.Select(x =>
                //{
                //    int index = command.Lessons.IndexOf(x);
                //    x.File = new();
                //    x.File.File = files[index];
                //    return x;
                //}).ToList(),
                //Description = command.Description,
                //Title = command.Title
            }));
        }
        [HttpGet("{id}/course-sections")]
        public async Task<IActionResult> GetCourseSectionByCourseId([FromRoute] Guid id)
        {
            return Ok(await mediator.Send(new GetCourseSectionByCourseIdQuery()
            {
                CourseId = id
            }));
        }
        [HttpPost("{id}/learning-materials")]
        public async Task<IActionResult> CreateLearningMaterials([FromForm] LearningMaterialCreateModel command,
            [FromRoute] Guid id)
        {
            return Ok(await mediator.Send(new CreateLearningMaterialCommand()
            {
                CourseId = id,
                Model = command
            }));
        }

        [HttpGet("{id}/learning-materials")]
        public async Task<IActionResult> GetLearningMaterials([FromRoute] Guid id,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageIndex = 0)
        {
            return Ok(await mediator.Send(new GetLearningMaterialQuery()
            {
                CourseId = id,
                PageIndex = pageIndex,
                PageSize = pageSize,
            }));
        }
        /// <summary>
        /// Update course
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CourseUpdateModel updatedItem)
        {
            var result = await mediator.Send(new UpdateCourseCommand()
            {
                Model = updatedItem
            });
            if (result) return NoContent();
            else return BadRequest();
        }

        /// <summary>
        /// Create course
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseCommand command,
            [FromServices] IMediator mediator)
        {
            /*createcourseDTO.File = formFile;*/

            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}/course-participations")]
        public async Task<IActionResult> GetCourseParticipation([FromRoute] Guid id,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageIndex = 0)
        {
            var result = await mediator.Send(new GetCourseParticipationByCourse()
            {
                CourseId = id,
                PageIndex = pageIndex,
                PageSize = pageSize
            });
            return Ok(result);
        }

        /// <summary>
        /// Get course by Id
        /// </summary>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            return Ok(await mediator.Send(new GetCourseByIdQuery()
            {
                Id = id
            }));

        }
    }
}

using Amazon.S3.Model;
using its.gamify.api.Features.CourseParticipations;
using its.gamify.api.Features.CourseParticipations.Commands;
using its.gamify.api.Features.Courses.Commands;
using its.gamify.api.Features.CourseSections.Queries;
using its.gamify.core.Features.CourseCollections.Commands;
using its.gamify.core.Features.CourseReviews;
using its.gamify.core.Features.Courses.Commands;
using its.gamify.core.Features.Courses.Queries;
using its.gamify.core.Features.LearningMaterials.Queries;
using its.gamify.core.Models;
using its.gamify.core.Models.Courses;
using its.gamify.core.Models.ShareModels;
using its.gamify.domains.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace its.gamify.api.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class CourseController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Delete course
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await mediator.Send(new DeleteCourseCommand()
            {
                Id = id
            });
            return NoContent();
        }


        /// <summary>
        /// Get all course
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CourseQuery query)
        {
            var res = await mediator.Send(new GetAllCourseQuery()
            {
                CourseQuery = query
            });
            return Ok(res);

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

        [HttpGet("{id}/course-sections")]
        public async Task<IActionResult> GetCourseSectionByCourseId([FromRoute] Guid id)
        {
            return Ok(await mediator.Send(new GetCourseSectionByCourseIdQuery()
            {
                CourseId = id
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
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id,
            [FromBody] CourseUpdateModel updatedItem)
        {
            updatedItem.Id = id;
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
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("{id}/course-participations")]
        public async Task<IActionResult> JoinCourse([FromRoute] Guid id)
        {
            var courseParticipation = await mediator.Send(new JoinCourseCommand()
            {
                Id = id
            });
            return Ok(courseParticipation);
        }

        [HttpGet("{id}/course-participations")]
        [Authorize]
        public async Task<IActionResult> GetCourseParticipation([FromRoute] Guid id)
        {
            var result = await mediator.Send(new GetCourseParticipationByCourse()
            {
                CourseId = id,
                PageIndex = 0,
                PageSize = 10
            });
            return Ok(result);
        }
        /// <summary>
        /// Reactive course
        /// </summary>

        [HttpPut("{id}/re-active")]
        [Authorize(Roles = ROLE.TRAININGSTAFF)]
        public async Task<IActionResult> ReActiveCourse([FromRoute] Guid id, [FromBody] BaseReActiveModel model)
        {
            return Ok(await mediator.Send(new ReActiveCourseCommand()
            {
                Id = id,
                IsActive = model.IsActive
            }));

        }

        [HttpPut("{id}/course-collections")]
        [Authorize]
        public async Task<IActionResult> UpsertCourseCollection([FromRoute] Guid id)
        {
            return Ok(await mediator.Send(new UpsertCourseCollectionCommand()
            {
                CourseId = id,
            }));

        }

        [HttpGet("{id}/course-reviews")]
        [Authorize]
        public async Task<IActionResult> GetCourseReviews([FromRoute] Guid id, [FromQuery] FilterQuery filterQuery)
        {
            return Ok(await mediator.Send(new GetCourseReviewQuery()
            {
                CourseId = id,
                Filter = filterQuery
            }));
        }

    }
}
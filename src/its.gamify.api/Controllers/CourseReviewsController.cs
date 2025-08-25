using its.gamify.core.Features.CourseReviews.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace its.gamify.api.Controllers
{
    [ApiController]
    [Route("api/course-reviews")]
    public class CourseReviewsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> CreateCourseReview([FromBody] CreateReviewCommand command)
        {

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourseReview([FromRoute] Guid id)
        {

            await _mediator.Send(new DeleteReviewComamnd { Id = id });
            return NoContent();
        }

    }
}

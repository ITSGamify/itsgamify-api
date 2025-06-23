using its.gamify.core.Features.CourseResults.Queries;
using its.gamify.domains.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace its.gamify.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseResultsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CourseResultsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetCourseResultQuery query)
        {
            var result = await _mediator.Send(query);
            if (result == null || result.Datas == null || !result.Datas.Any())
                throw new InvalidOperationException("Danh sách CourseResult trống");
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetCourseResultByIdQuery { Id = id });
            if (result == null)
                throw new InvalidOperationException($"Không tìm thấy CourseResult với id: {id}");
            return Ok(result);
        }
    }
}

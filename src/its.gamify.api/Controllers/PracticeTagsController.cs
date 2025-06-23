using its.gamify.core.Features.PracticeTags.Queries;
using its.gamify.domains.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace its.gamify.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PracticeTagsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PracticeTagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetPracticeTagQuery query)
        {
            var result = await _mediator.Send(query);
            if (result == null || result.Datas == null || !result.Datas.Any())
                throw new InvalidOperationException("Danh sách PracticeTag trống");
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetPracticeTagByIdQuery { Id = id });
            if (result == null)
                throw new InvalidOperationException($"Không tìm thấy PracticeTag với id: {id}");
            return Ok(result);
        }
    }
}

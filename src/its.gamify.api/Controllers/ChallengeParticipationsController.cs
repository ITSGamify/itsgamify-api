using its.gamify.core.Features.ChallengeParticipations.Queries;
using its.gamify.domains.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace its.gamify.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChallengeParticipationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ChallengeParticipationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetChallengeParticipationQuery query)
        {
            var result = await _mediator.Send(query);
            if (result == null || result.Datas == null || !result.Datas.Any())
                throw new InvalidOperationException("Danh sách ChallengeParticipation trống");
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetChallengeParticipationByIdQuery { Id = id });
            if (result == null)
                throw new InvalidOperationException($"Không tìm thấy ChallengeParticipation với id: {id}");
            return Ok(result);
        }
    }
}

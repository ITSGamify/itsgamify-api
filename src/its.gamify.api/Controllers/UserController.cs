using its.gamify.api.Features.Users.Commands;
using its.gamify.api.Features.Users.Queries;
using its.gamify.core.Features.CourseResults;
using its.gamify.core.Features.UserChallengeHistories.Queries;
using its.gamify.core.Features.UserMetrics;
using its.gamify.core.Models.ShareModels;
using its.gamify.core.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace its.gamify.api.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class UserController(IMediator mediator) : BaseController
    {
        private readonly IMediator mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAllUser([FromQuery] UserFilterQuery filter)
        {
            return Ok(await mediator.Send(new GetAllUserQuery()
            {
                FilterQuery = filter
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            return Ok(await mediator.Send(new GetUserByIdQuery()
            {
                Id = id
            }));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateModel model)
        {
            var res = await mediator.Send(new CreateUserCommand()
            {
                Model = model
            });
            if (res is not null)
            {
                return Ok(res);
            }
            return StatusCode(500);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserUpdateModel model)
        {
            var res = await mediator.Send(new UpdateUserCommand()
            {
                Id = id,
                Model = model
            });
            return res ? NoContent() : StatusCode(500);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var res = await mediator.Send(new DeleteUserCommand()
            {
                Id = id
            });
            return res ? NoContent() : StatusCode(500);

        }

        [HttpDelete("delete-range")]
        public async Task<IActionResult> DeleteRange(List<Guid> ids)
        {
            await mediator.Send(new DeleteUserRangeCommand()
            {
                Ids = ids
            });
            return NoContent();
        }


        [HttpGet("{id}/statistic")]
        public async Task<IActionResult> GetStatistic([FromRoute] Guid id, [FromQuery] Guid Quaterid)
        {
            var res = await mediator.Send(new GetUserStatistic()
            {
                UserId = id,
                QuarterId = Quaterid
            });
            return Ok(res);

        }
        [HttpGet("{id}/course-results")]
        public async Task<IActionResult> GetAllCourseResult([FromRoute] Guid id, [FromQuery] FilterQueryExtend query)
        {

            return Ok(await mediator.Send(new GetCourseResultByUserIdQuery()
            {
                UserId = id,
                FilterQuery = query
            }));
        }
        [HttpGet("{id}/user-metrics")]
        [Authorize]
        public async Task<IActionResult> GetMetric(Guid id)
        {
            var res = await mediator.Send(new UserMetricQuery
            {
                Id = id
            });
            return Ok(res);

        }
        [HttpGet("{id}/challenge-histories")]
        [Authorize]
        public async Task<IActionResult> GetAllChallengeHistory([FromQuery] FilterQueryExtend filter, Guid id)
        {
            var res = await mediator.Send(new GetAllUserChallengeHistoryQuery
            {
                Filter = filter,
                UserId = id
            });

            return Ok(res);

        }
    }
}

using its.gamify.api.Features.Users.Commands;
using its.gamify.api.Features.Users.Queries;
using its.gamify.core.Models.ShareModels;
using its.gamify.core.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace its.gamify.api.Controllers
{

    public class UsersController : BaseController
    {
        private readonly IMediator mediator;
        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUser([FromQuery] FilterQuery filter)
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

    }
}

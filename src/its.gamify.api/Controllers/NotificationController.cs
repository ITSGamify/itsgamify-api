using its.gamify.core.Features.Notifications.Commands;
using its.gamify.core.Features.Notifications.Queries;
using its.gamify.core.Models.ShareModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace its.gamify.api.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class NotificationController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] FilterQuery filter)
        {
            return Ok(await mediator.Send(new GetAllNotificationQuery
            {
                Filter = filter
            }));

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationCreateModel model)
        {
            return Ok(await mediator.Send(new CreateNotificationCommand
            {
                Model = model
            }));

        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateNotification([FromRoute] Guid id, [FromBody] NotificationUpdateModel model)
        {
            return Ok(await mediator.Send(new UpdateNotificationCommand
            {
                Id = id,
                Model = model
            }));

        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ReadAllNotification([FromBody] NotificationUpdateModel model)
        {
            return Ok(await mediator.Send(new ReadAllNotificationCommand
            {
                Model = model
            }));

        }
    }
}

using its.gamify.core.Features.WishLists.Queries;
using its.gamify.domains.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace its.gamify.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WishListsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public WishListsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetWishListQuery query)
        {
            var result = await _mediator.Send(query);
            if (result == null || result.Datas == null || !result.Datas.Any())
                throw new InvalidOperationException("Danh sách WishList trống");
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetWishListByIdQuery { Id = id });
            if (result == null)
                throw new InvalidOperationException($"Không tìm thấy WishList với id: {id}");
            return Ok(result);
        }
    }
}

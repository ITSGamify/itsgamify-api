using its.gamify.core.Features.LearningMaterials.Queries;
using its.gamify.domains.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace its.gamify.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LearningMaterialsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LearningMaterialsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetLearningMaterialQuery query)
        {
            var result = await _mediator.Send(query);
            if (result == null || result.Datas == null || !result.Datas.Any())
                throw new InvalidOperationException("Danh sách LearningMaterial trống");
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetLearningMaterialByIdQuery { Id = id });
            if (result == null)
                throw new InvalidOperationException($"Không tìm thấy LearningMaterial với id: {id}");
            return Ok(result);
        }
    }
}

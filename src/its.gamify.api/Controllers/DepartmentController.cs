using its.gamify.api.Extensions;
using its.gamify.api.Features.Departments.Commands;
using its.gamify.api.Features.Departments.Queries;
using its.gamify.core.Features.AvailablesData;
using its.gamify.core.Models;
using its.gamify.core.Models.Departments;
using its.gamify.core.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;




namespace its.gamify.api.Controllers
{

    [Route("api/[controller]s")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly IMediator mediator;
        public DepartmentController(IDepartmentService DepartmentService, IMediator mediator)
        {
            _departmentService = DepartmentService;
            this.mediator = mediator;
        }
        /// <summary>
        /// Delete Department
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _departmentService.Delete(id);
            if (result) return Ok("Delete Successfully");
            else return BadRequest("Deleted Failed");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteRage([FromBody] List<Guid> ids)
        {
            var result = await _departmentService.DeleteRange(ids);
            return NoContent();
        }


        [HttpGet]
        [ProcessOrderBy]
        public async Task<IActionResult> GetAll([FromQuery] DepartmentQueryDto queryDto)
        {
            var query = HttpContext.Request.Query;

            for (int i = 0; ; i++)
            {
                var columnKey = $"order_by[{i}][order_column]";
                var dirKey = $"order_by[{i}][order_dir]";

                if (!query.ContainsKey(columnKey))
                    break;

                Console.WriteLine($"columnKey {query[columnKey].ToString()}");
                Console.WriteLine($"dirKey {query[dirKey].ToString()}");

                queryDto.OrderBy.Add(new OrderByItem
                {
                    OrderColumn = query[columnKey].ToString(),
                    OrderDir = query.ContainsKey(dirKey) ? query[dirKey].ToString() : "ASC"
                });
            }

            var result = await _departmentService.GetAll(queryDto);
            return Ok(
                new { data = result.Item2, pagination = result.Item1 }
            );
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DepartmentUpdateModel updatedItem)
        {
            var result = await _departmentService.Update(updatedItem);
            if (result) return Ok("Cập nhật thành công");
            else return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DepartmentCreateModel createItem)
        {
            var result = await _departmentService.Create(createItem);
            if (result is null)
            {
                return BadRequest("Can not create Department");

            }
            else return Ok(result);
        }
    


        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await mediator.Send(new GetDepartmentByIdQuery()
            {
                Id = id
            }));
        }
        /// <summary>
        /// Delete list Department 
        /// </summary>
        [HttpDelete("delete-range")]
        public async Task<IActionResult> DeleteRange(List<Guid> ids)
        {
            var res = await mediator.Send(new DeleteRangeDepartmentCommand()
            {
                Ids = ids
            });
            return res ? NoContent() : StatusCode(500);
        }
    }
}

using its.gamify.core.Models.Departments;
using its.gamify.core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace its.gamify.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService DepartmentService)
        {
            _departmentService = DepartmentService;
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

        /// <summary>
        /// Get all Department
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 0,
                                        [FromQuery] int pageSize = 10,
                                        [FromQuery] string searchTerm = ""
                                        )
        {
            var result = await _departmentService.GetAll(pageNumber, pageSize, searchTerm);
            if (result.Count() > 0) { return Ok(result); }
            else return BadRequest();

        }

        /// <summary>
        /// Update product
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] DepartmentUpdateModel updatedItem)
        {
            var result = await _departmentService.Update(updatedItem);
            if (result) return NoContent();
            else return BadRequest();
        }

        /// <summary>
        /// Create product
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] DepartmentCreateModel createItem)
        {
            /*createProductDTO.File = formFile;*/
            var result = await _departmentService.Create(createItem);
            if (result is null)
            {
                return BadRequest("Can not create Department");

            }
            else return Ok(result);
        }


        /// <summary>
        /// Get product by Id
        /// </summary>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _departmentService.GetDepartment(id);
            return Ok(result);
        }
    }
}

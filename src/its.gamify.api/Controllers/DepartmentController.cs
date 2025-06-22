using its.gamify.core.Features.AvailablesData;
using its.gamify.core.Models.Departments;
using its.gamify.core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace its.gamify.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private Ultils data;
        public DepartmentController(IDepartmentService DepartmentService, Ultils data)
        {
            _departmentService = DepartmentService;
            this.data = data;
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
            return Ok(data.courses);

        }

        /// <summary>
        /// Update department
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] DepartmentUpdateModel updatedItem)
        {
            var result = await _departmentService.Update(updatedItem);
            if (result) return NoContent();
            else return BadRequest();
        }

        /// <summary>
        /// Create department
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] DepartmentCreateModel createItem)
        {
            /*createdepartmentDTO.File = formFile;*/
            var result = await _departmentService.Create(createItem);
            if (result is null)
            {
                return BadRequest("Can not create Department");

            }
            else return Ok(result);
        }


        /// <summary>
        /// Get department by Id
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

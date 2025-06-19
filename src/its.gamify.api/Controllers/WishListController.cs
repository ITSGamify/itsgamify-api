using Microsoft.AspNetCore.Mvc;

namespace its.gamify.api.Controllers;

public class WishListController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok();
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok();
    }
}

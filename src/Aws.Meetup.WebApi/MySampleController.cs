using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Aws.Meetup.WebApi;

[ApiController]
[Route("api/v1/samples")]
public class MySampleController : ControllerBase
{
    [HttpGet]
    [Route("{id:long}")]
    public IActionResult GetStuff(long id)
    {
        return Ok(new {test = "cool"});
    }

    [HttpPost]
    public IActionResult Create([FromBody] MyObject myObject)
    {
        return Created();
    }

    [HttpDelete]
    [Route("{id:long}")]
    public IActionResult Delete(long id)
    {
        return NoContent();
    }
}

public class MyObject
{
    [MinLength(10)]
    public required string Type { get; init; }
}
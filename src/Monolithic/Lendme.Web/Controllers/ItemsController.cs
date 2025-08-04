using Asp.Versioning;
using Lendme.Core.Entities.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace Lendme.Web.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Produces("application/json")]
[ApiVersion("1")]
public class ItemsController : ControllerBase
{
    // GET
    [HttpGet]
    public ActionResult<List<Category>> Index()
    {
        return new List<Category>()
        {
            new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Category 1"
            }
        };
    }
}

// Временный контроллер для тестирования
[Route("api/test")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Get()
    {
        return "API works!";
    }
}

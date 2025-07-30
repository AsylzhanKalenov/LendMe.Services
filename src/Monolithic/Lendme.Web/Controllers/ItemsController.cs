using Asp.Versioning;
using Lendme.Core.Entities.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace Lendme.Web.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[ApiVersion("1.0")]
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
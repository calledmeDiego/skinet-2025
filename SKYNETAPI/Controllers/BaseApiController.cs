using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SKYNETAPI.RequestHelpers;
using SKYNETCORE.Entities;
using SKYNETCORE.Interfaces;

namespace SKYNETAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{

    protected async Task<IActionResult> CreatePagedResult<T>(IGenericRepository<T> repository, ISpecification<T> spec, int pageIndex, int pageSize) where T : BaseEntity
    {
        var items = await repository.ListAsync(spec);
        var count = await repository.CountAsync(spec);

        var pagination = new Pagination<T>(pageIndex, pageSize, count, items);

        return Ok(pagination);

    }
}

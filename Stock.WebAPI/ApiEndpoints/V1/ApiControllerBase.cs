using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Stock.WebAPI.ApiEndpoints.V1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    
}

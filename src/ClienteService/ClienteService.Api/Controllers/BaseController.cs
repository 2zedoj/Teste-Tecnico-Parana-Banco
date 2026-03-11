using Microsoft.AspNetCore.Mvc;
using Abstraction = ClienteService.Domain.Abstraction;

namespace ClienteService.Api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        public BaseController()
        {
        }

        public IActionResult CreateResult<TDto>(Abstraction.Result<TDto> result)
            where TDto : Abstraction.IResult
        {
            var _localizer = HttpContext?.RequestServices?.GetService<IResourceLocalizer>();
            // Garante que o Content-Type seja JSON
            //Response.ContentType = "application/json; charset=utf-8";

            if (result.Error != null && result.Error.Count > 0)
            {
                for (int i = 0; i < result.Error.Count; i++)
                {
                    result.Error[i] = _localizer?.Localize(result.Error[i]) ?? result.Error[i];
                }
            }

            return result.StatusCode == 204
                ? new ObjectResult(null) { StatusCode = 204 }
                : new ObjectResult(result) { StatusCode = result.StatusCode };
        }
    }
}

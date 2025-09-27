using HelsiTestTask.Api.Common.Responses;
using HelsiTestTask.Application.Mediator.Core;
using Microsoft.AspNetCore.Mvc;

namespace HelsiTestTask.Api.Controllers;

[ApiController]
[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
public class HelsiTestTaskController: ControllerBase
{
    protected readonly IMediator Mediator;

    protected HelsiTestTaskController(IMediator mediator)
    {
        Mediator = mediator;
    }
}

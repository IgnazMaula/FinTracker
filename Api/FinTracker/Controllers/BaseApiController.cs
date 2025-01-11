using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace FinTracker.Api.Controllers;

public class BaseApiController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
}
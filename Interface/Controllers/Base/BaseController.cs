using Microsoft.AspNetCore.Mvc;
using SartainStudios.Log;

namespace Interface.Controllers.Base;

public class BaseController : ControllerBase
{
    private ILog _log;

    protected ILog Log =>
        _log ??= HttpContext?.RequestServices.GetService<ILog>();
}